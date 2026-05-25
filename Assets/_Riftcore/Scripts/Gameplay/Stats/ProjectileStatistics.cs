using System;
using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class ProjectileStatistics
    {
        [field: SerializeField, Min(0)] public float ProjectileCount { get; set; } // 0 - ...
        [field: SerializeField, Min(1)] public float ProjectileSize { get; set; } // 1.0x
        [field: SerializeField, Min(1)] public float ProjectileSpeed { get; set; } // 1.0x
        [field: SerializeField, Min(0)] public float ProjectileFiringRange { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public float ProjectileBounceCount { get; set; } // 0 - ...
        [field: SerializeField, Min(0)] public float ProjectilePenetration { get; set; } // 0 - ...
        [field: SerializeField, Min(1)] public float Duration { get; set; } // 1.0x
        
        public ProjectileStatistics(ProjectileStatistics projectileStatistics)
        {
            ProjectileCount = projectileStatistics.ProjectileCount;
            ProjectileSize = projectileStatistics.ProjectileSize;
            ProjectileSpeed = projectileStatistics.ProjectileSpeed;
            ProjectileFiringRange  = projectileStatistics.ProjectileFiringRange;
            ProjectileBounceCount = projectileStatistics.ProjectileBounceCount;
            ProjectilePenetration = projectileStatistics.ProjectilePenetration;
            Duration = projectileStatistics.Duration;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.ProjectileCount:
                    ProjectileCount = statModifierData.Apply(ProjectileCount);
                    break;
                
                case StatType.ProjectileSize:
                    ProjectileSize = statModifierData.Apply(ProjectileSize);
                    break;
                
                case StatType.ProjectileSpeed:
                    ProjectileSpeed = statModifierData.Apply(ProjectileSpeed);
                    break;
                
                case StatType.ProjectileFiringRange:
                    ProjectileFiringRange = statModifierData.Apply(ProjectileFiringRange);
                    break;
                
                case StatType.ProjectileBounceCount:
                    ProjectileBounceCount = statModifierData.Apply(ProjectileBounceCount);
                    break;
                
                case StatType.ProjectilePenetration:
                    ProjectilePenetration = statModifierData.Apply(ProjectilePenetration);
                    break;
                
                case StatType.Duration:
                    Duration = statModifierData.Apply(Duration);
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.ProjectileCount:
                    value = ProjectileCount;
                    return true;
                
                case StatType.ProjectileSize:
                    value = ProjectileSize;
                    return true;
                
                case StatType.ProjectileSpeed:
                    value = ProjectileSpeed;
                    return true;
                
                case StatType.ProjectileFiringRange:
                    value = ProjectileFiringRange;
                    return true;
                
                case StatType.ProjectileBounceCount:
                    value = ProjectileBounceCount;
                    return true;
                
                case StatType.ProjectilePenetration:
                    value = ProjectilePenetration;
                    return true;
                
                case StatType.Duration:
                    value = Duration;
                    return true;
                
                default:
                    return false;
            }
        }
    }
}