using System.Collections.Generic;
using Riftcore.Gameplay.Inventory.Data;

namespace Riftcore.Gameplay.Skills.Data
{
    public sealed class SkillReward
    {
        public ItemData ItemData { get; }
        public SkillRarity SkillRarity { get; }
        public IReadOnlyList<SkillData> SkillDatas { get; }
        public bool IsNewItem { get; }
        
        public int CurrentLevel { get; }
        public int NextLevel => CurrentLevel + 1;

        public SkillReward(ItemData itemData, SkillRarity skillRarity, IReadOnlyList<SkillData> skillDatas, bool isNewItem, int currentLevel)
        {
            ItemData = itemData;
            SkillRarity = skillRarity;
            SkillDatas = skillDatas;
            IsNewItem = isNewItem;
            CurrentLevel = currentLevel;
        }
    }
}