using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Skills.Data;
using Riftcore.Gameplay.Skills.Stats;

namespace Riftcore.Gameplay.Skills.Runtime
{
    public sealed class SkillRewardApplier
    {
        private readonly GameContext _gameContext;

        public SkillRewardApplier(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Apply(SkillReward skillReward)
        {
            var player = _gameContext.Player;
            
            var item = player.PlayerInventory.GetItem(skillReward.ItemData);
            if (item == null)
            {
                item = new InventoryItem(skillReward.ItemData);
                player.PlayerInventory.AddItem(item);
            }
            else
            {
                item.LevelUp();
            }

            foreach (var skillData in skillReward.SkillDatas)
                ApplySkill(skillReward, skillData);
        }

        private void ApplySkill(SkillReward skillReward, SkillData skillData)
        {
            var player = _gameContext.Player;
            
            //var rarity = skillReward.IsNewItem ? SkillRarity.Common : skillReward.SkillRarity;
            var modifier = skillData.GetModifier(skillReward.SkillRarity);

            if (skillReward.ItemData.ItemCategory == ItemCategory.Weapon)
                ApplyWeaponModifier(player, skillReward.ItemData, modifier);
            else
                ApplyPlayerModifier(player, modifier);
        }

        private void ApplyWeaponModifier(Player player, ItemData itemData, StatModifierData statModifierData)
        {
            var weapon = player.PlayerWeaponController.GetWeapon(itemData);

            weapon?.ApplyModifier(statModifierData);
        }

        private void ApplyPlayerModifier(Player player, StatModifierData statModifierData)
        {
            player.GameStatistics.ApplyModifier(statModifierData);
        }
    }
}