using System;
using System.Collections.Generic;
using System.Linq;
using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Skills.Data;
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

        public List<SkillReward> GenerateChoices(int count)
        {
            var player = _gameContext.Player;
            var inventory = player.PlayerInventory;
            
            var availableItems = GetAvailableItems(inventory);
            var result = new List<SkillReward>();

            for (int i = 0; i < count && availableItems.Count > 0; i++)
            {
                int index = Random.Range(0, availableItems.Count);
                var itemData = availableItems[index];
                
                availableItems.RemoveAt(index);
                result.Add(CreateReward(player, itemData));
            }

            return result;
        }

        private List<ItemData> GetAvailableItems(PlayerInventory playerInventory)
        {
            var result = new List<ItemData>(_itemConfigDatabase.Items);

            result.RemoveAll(itemData =>
            {
                var item = playerInventory.GetItem(itemData);
                return item != null && item.IsMaxLevel;
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

        private SkillReward CreateReward(Player player, ItemData itemData)
        {
            var item = player.PlayerInventory.GetItem(itemData);
            bool isNewItem = item == null;
            
            int currentLevel = item?.Level ?? 0;

            float luck = player.GameStatistics.LevelStatistics.Luck;

            SkillRarity rarity = isNewItem 
                ? _rarityDatabase.GetNewItemRarity() 
                : _rarityDatabase.GetRandomRarity(luck);

            var skills = GetRandomSkills(itemData, isNewItem);

            return new SkillReward(itemData, rarity, skills, isNewItem, currentLevel);
        }

        private IReadOnlyList<SkillData> GetRandomSkills(ItemData itemData, bool isNewItem)
        {
            if (isNewItem)
                return new List<SkillData>();
            
            var skills = new List<SkillData>(itemData.AvailableSkills);
            var result = new List<SkillData>();

            int maxCount = Mathf.Min(itemData.MaxNumberDropSkills, skills.Count);
            int count = GetRandomSkillCount(itemData.MinNumberDropSkills, maxCount);

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
    }
}