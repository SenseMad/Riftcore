using System;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class CombatStatistics
    {
        [field: SerializeField, Min(1)] public float Damage { get; set; } = 1.0f; // 1.0x
        [field: SerializeField, Min(1)] public float AttackSpeed { get; set; } = 1.0f; // 1.0x
        [field: SerializeField, Range(0, 100)] public float CritChance { get; set; } // 0-100%
        [field: SerializeField, Min(1)] public float CritMultiplier { get; set; } = 1.0f; // 1.0x
        
        public CombatStatistics(CombatStatistics combatStatistics)
        {
            Damage = combatStatistics.Damage;
            AttackSpeed = combatStatistics.AttackSpeed;
            CritChance = combatStatistics.CritChance;
            CritMultiplier = combatStatistics.CritMultiplier;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.Damage:
                    Damage = statModifierData.Apply(Damage);
                    break;
                
                case StatType.AttackSpeed:
                    AttackSpeed = statModifierData.Apply(AttackSpeed);
                    break;
                
                case StatType.CritChance:
                    CritChance = statModifierData.Apply(CritChance);
                    break;
                
                case StatType.CritDamage:
                    CritMultiplier = statModifierData.Apply(CritMultiplier);
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.Damage:
                    value = Damage;
                    return true;
                
                case StatType.AttackSpeed:
                    value = AttackSpeed;
                    return true;
                
                case StatType.CritChance:
                    value = CritChance;
                    return true;
                
                case StatType.CritDamage:
                    value = CritMultiplier;
                    return true;
                
                default:
                    return false;
            }
        }
    }
}