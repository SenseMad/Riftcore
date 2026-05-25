using System;
using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class HealthStatistics
    {
        [field: SerializeField, Min(0)] public float MaxHealth { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public float RegenerationHealth { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public float Armor { get; set; } // %
        [field: SerializeField, Min(0)] public float Shield { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public float DodgingChance { get; set; } // %
        [field: SerializeField, Min(0)] public float Vampirism { get; set; } // %
        [field: SerializeField, Min(0)] public float Spikes { get; set; } // 0 - ...
        
        public HealthStatistics(HealthStatistics healthStatistics)
        {
            MaxHealth = healthStatistics.MaxHealth;
            RegenerationHealth = healthStatistics.RegenerationHealth;
            Armor = healthStatistics.Armor;
            Shield = healthStatistics.Shield;
            DodgingChance = healthStatistics.DodgingChance;
            Vampirism = healthStatistics.Vampirism;
            Spikes = healthStatistics.Spikes;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.MaxHealth:
                    MaxHealth = Mathf.RoundToInt(statModifierData.Apply(MaxHealth));
                    break;
                
                case StatType.RegenerationHealth:
                    RegenerationHealth = statModifierData.Apply(RegenerationHealth);
                    break;
                
                case StatType.Armor:
                    Armor = statModifierData.Apply(Armor);
                    break;
                
                case StatType.Shield:
                    Shield = statModifierData.Apply(Shield);
                    break;

                case StatType.DodgingChance:
                    DodgingChance = statModifierData.Apply(DodgingChance);
                    break;

                case StatType.Vampirism:
                    Vampirism = statModifierData.Apply(Vampirism);
                    break;

                case StatType.Spikes:
                    Spikes = statModifierData.Apply(Spikes);
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.MaxHealth:
                    value = MaxHealth;
                    return true;
                
                case StatType.RegenerationHealth:
                    value = RegenerationHealth;
                    return true;
                
                case StatType.Armor:
                    value = Armor;
                    return true;
                
                case StatType.Shield:
                    value = Shield;
                    return true;
                
                case StatType.DodgingChance:
                    value = DodgingChance;
                    return true;
                
                case StatType.Vampirism:
                    value = Vampirism;
                    return true;
                
                case StatType.Spikes:
                    value = Spikes;
                    return true;
                
                default:
                    return false;
            }
        }
    }
}