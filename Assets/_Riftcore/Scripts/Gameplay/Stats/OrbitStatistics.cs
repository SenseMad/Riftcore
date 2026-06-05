using System;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class OrbitStatistics
    {
        [field: SerializeField, Min(0)] public float OrbitObjectCount { get; set; }
        [field: SerializeField, Min(0)] public float OrbitRadius { get; set; }
        [field: SerializeField, Min(0)] public float OrbitRotationSpeed { get; set; }
        
        public OrbitStatistics(OrbitStatistics orbitStatistics)
        {
            OrbitObjectCount = orbitStatistics.OrbitObjectCount;
            OrbitRadius = orbitStatistics.OrbitRadius;
            OrbitRotationSpeed = orbitStatistics.OrbitRotationSpeed;
        }
        
        public void ApplyModifier(StatModifierData statModifierData)
        {
            switch (statModifierData.StatType)
            {
                case StatType.OrbitObjectCount:
                    OrbitObjectCount = statModifierData.Apply(OrbitObjectCount);
                    break;
                
                case StatType.OrbitRadius:
                    OrbitRadius = statModifierData.Apply(OrbitRadius);
                    break;
                
                case StatType.OrbitRotationSpeed:
                    OrbitRotationSpeed = statModifierData.Apply(OrbitRotationSpeed);
                    break;
            }
        }
        
        public bool TryGetValue(StatType statType, out float value)
        {
            value = 0f;

            switch (statType)
            {
                case StatType.OrbitObjectCount:
                    value = OrbitObjectCount;
                    return true;
                
                case StatType.OrbitRadius:
                    value = OrbitRadius;
                    return true;
                
                case StatType.OrbitRotationSpeed:
                    value = OrbitRotationSpeed;
                    return true;
                
                default:
                    value = 0f;
                    return false;
            }
        }
    }
}