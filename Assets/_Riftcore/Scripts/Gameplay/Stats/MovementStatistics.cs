using System;
using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class MovementStatistics
    {
        [field: SerializeField, Min(1)] public float MoveSpeed { get; set; } // 1.0x
        [field: SerializeField, Min(0)] public float JumpHeight { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public int NumberJumps { get; set; } // 0 - ...
        
        public MovementStatistics(MovementStatistics movementStatistics)
        {
            MoveSpeed = movementStatistics.MoveSpeed;
            JumpHeight = movementStatistics.JumpHeight;
            NumberJumps = movementStatistics.NumberJumps;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.MoveSpeed:
                    MoveSpeed = statModifierData.Apply(MoveSpeed);
                    break;
                
                case StatType.JumpHeight:
                    JumpHeight = statModifierData.Apply(JumpHeight);
                    break;
                
                case StatType.NumberJumps:
                    NumberJumps = Mathf.RoundToInt(statModifierData.Apply(NumberJumps));
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.MoveSpeed:
                    value = MoveSpeed;
                    return true;
                
                case StatType.JumpHeight:
                    value = JumpHeight;
                    return true;
                
                case StatType.NumberJumps:
                    value = NumberJumps;
                    return true;
                
                default:
                    return false;
            }
        }
    }
}