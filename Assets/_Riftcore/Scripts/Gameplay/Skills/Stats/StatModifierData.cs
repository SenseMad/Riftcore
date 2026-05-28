using System;
using UnityEngine;

namespace Riftcore.Gameplay.Skills.Stats
{
    [Serializable]
    public class StatModifierData
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public StatOperation StatOperation { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        
        public bool HasMaxValue { get; }
        public float MaxValue { get; }

        public StatModifierData(StatType statType, StatOperation statOperation, float value, bool hasMaxValue = false, float maxValue = 0f)
        {
            StatType = statType;
            StatOperation = statOperation;
            Value = value;
            HasMaxValue = hasMaxValue;
            MaxValue = maxValue;
        }

        public float Apply(float currentValue)
        {
            float newValue = StatOperation switch
            {
                StatOperation.Add => currentValue + Value,
                StatOperation.Multiply => currentValue * Value,
                _ => currentValue
            };
            
            if (HasMaxValue)
                newValue = Mathf.Min(newValue, MaxValue);
            
            return newValue;
        }
    }
}