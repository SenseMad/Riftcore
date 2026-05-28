using System;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class LevelStatistics
    {
        [field: SerializeField, Min(0)] public float Difficulty { get; set; } // %
        [field: SerializeField, Min(0)] public float Luck { get; set; } // %
        [field: SerializeField, Min(0)] public float CollectingRadius { get; set; } // 0 ...
        [field: SerializeField, Min(1)] public float MultiplierExperience { get; set; } = 1.0f; // 1.0x

        public LevelStatistics(LevelStatistics levelStatistics)
        {
            Difficulty = levelStatistics.Difficulty;
            Luck = levelStatistics.Luck;
            CollectingRadius = levelStatistics.CollectingRadius;
            MultiplierExperience = levelStatistics.MultiplierExperience;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.Difficulty:
                    Difficulty = statModifierData.Apply(Difficulty);
                    break;
                
                case StatType.Luck:
                    Luck = statModifierData.Apply(Luck);
                    break;
                
                case StatType.CollectingRadius:
                    CollectingRadius = statModifierData.Apply(CollectingRadius);
                    break;
                
                case StatType.MultiplierExperience:
                    MultiplierExperience = statModifierData.Apply(MultiplierExperience);
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.Difficulty:
                    value = Difficulty;
                    return true;
                
                case StatType.Luck:
                    value = Luck;
                    return true;
                
                case StatType.CollectingRadius:
                    value = CollectingRadius;
                    return true;
                
                case StatType.MultiplierExperience:
                    value = MultiplierExperience;
                    return true;
                
                default:
                    return false;
            }
        }
    }
}