using System;
using System.Collections.Generic;
using System.Linq;
using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Skills.Data;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Riftcore.Gameplay.Skills.Runtime
{
    public sealed class SkillSelectionService
    {
        private readonly ItemConfigDatabase _itemConfigDatabase;
        private readonly ItemCategoryDatabase _itemCategoryDatabase;
        private readonly RarityDatabase _rarityDatabase;
        private readonly GameContext _gameContext;

        public SkillSelectionService(ItemConfigDatabase itemConfigDatabase, ItemCategoryDatabase itemCategoryDatabase, RarityDatabase rarityDatabase,
            GameContext gameContext)
        {
            _itemConfigDatabase = itemConfigDatabase;
            _itemCategoryDatabase = itemCategoryDatabase;
            _rarityDatabase = rarityDatabase;
            _gameContext = gameContext;
        }
        
        public event Action OnSkillSelected;

        public List<SkillReward> GenerateChoices(int count)
        {
            var player = _gameContext.Player;
            
            var availableItems = GetAvailableItems(player);
            var result = new List<SkillReward>();

            while (result.Count < count && availableItems.Count > 0)
            {
                int index = Random.Range(0, availableItems.Count);
                var itemData = availableItems[index];
                
                availableItems.RemoveAt(index);

                var reward = CreateReward(player, itemData);
                if (!reward.IsNewItem && reward.SkillDatas.Count == 0)
                    continue;

                result.Add(reward);
            }

            return result;
        }
        
        public void SelectSkill(SkillReward reward)
        {
            OnSkillSelected?.Invoke();
        }

        private List<ItemData> GetAvailableItems(Player player)
        {
            var result = new List<ItemData>(_itemConfigDatabase.Items);
            var playerInventory = player.PlayerInventory;

            result.RemoveAll(itemData =>
            {
                var item = playerInventory.GetItem(itemData);
                if (item != null && item.IsMaxLevel)
                    return true;

                if (item != null && AreAllSkillsMaxed(player, itemData))
                    return true;

                return false;
            });

            foreach (ItemCategory category in Enum.GetValues(typeof(ItemCategory)))
            {
                int maxItems = _itemCategoryDatabase.GetMaxItems(category);
                
                var ownedItems = playerInventory.GetItemsByCategoty(category);
                if (ownedItems.Count < maxItems)
                    continue;

                var ownedItemDatas = ownedItems.Select(x => x.ItemData).ToHashSet();
                
                result.RemoveAll(itemData => itemData.ItemCategory == category && !ownedItemDatas.Contains(itemData));
            }

            return result;
        }
        
        private bool AreAllSkillsMaxed(Player player, ItemData itemData)
        {
            if (itemData.AvailableSkills.Count == 0)
                return false;

            foreach (var skillData in itemData.AvailableSkills)
            {
                if (!IsSkillMaxed(player, itemData, skillData))
                    return false;
            }
            
            return true;
        }

        private SkillReward CreateReward(Player player, ItemData itemData)
        {
            var item = player.PlayerInventory.GetItem(itemData);
            bool isNewItem = item == null;
            
            int currentLevel = item?.Level ?? 0;

            float luck = player.GameStatistics.LevelStatistics.Luck;

            SkillRarity rarity = isNewItem ? SkillRarity.Common : _rarityDatabase.GetRandomRarity(luck);

            var skills = GetRandomSkills(player, itemData, isNewItem);

            return new SkillReward(itemData, rarity, skills, isNewItem, currentLevel);
        }

        private IReadOnlyList<SkillData> GetRandomSkills(Player player, ItemData itemData, bool isNewItem)
        {
            if (isNewItem && itemData.ItemCategory == ItemCategory.Weapon)
                return new List<SkillData>();
            
            var skills = new List<SkillData>(itemData.AvailableSkills);
            var result = new List<SkillData>();

            skills.RemoveAll(skill => IsSkillMaxed(player, itemData, skill));

            int maxCount = Mathf.Min(itemData.MaxNumberDropSkills, skills.Count);
            int minCount = Mathf.Min(itemData.MinNumberDropSkills, maxCount);
            
            int count = isNewItem ? minCount : GetRandomSkillCount(minCount, maxCount);

            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, skills.Count);
                
                result.Add(skills[index]);
                skills.RemoveAt(index);
            }

            return result;
        }

        private int GetRandomSkillCount(int minCount, int maxCount)
        {
            int result = minCount;

            for (int i = minCount + 1; i <= maxCount; i++)
            {
                float chance = 1f / i;

                if (Random.value <= chance)
                    result = i;
            }

            return result;
        }

        private bool IsSkillMaxed(Player player, ItemData itemData, SkillData skillData)
        {
            if (!skillData.HasMaxValue)
                return false;

            IStatProvider statProvider = GetStatProvider(player, itemData);
            if (statProvider == null)
                return false;

            if (!statProvider.TryGetStatValue(skillData.StatType, out float currentValue))
                return false;
            
            return currentValue >= skillData.MaxValue;
        }

        private IStatProvider GetStatProvider(Player player, ItemData itemData)
        {
            if (itemData.ItemCategory == ItemCategory.Weapon)
                return player.PlayerWeaponController.GetWeapon(itemData);

            return player.GameStatistics;
        }
    }
}