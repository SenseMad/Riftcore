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

        public StatModifierData(StatType statType, StatOperation statOperation, float value)
        {
            StatType = statType;
            StatOperation = statOperation;
            Value = value;
        }

        public float Apply(float currentValue)
        {
            return StatOperation switch
            {
                StatOperation.Add => currentValue + Value,
                StatOperation.Multiply => currentValue * Value,
                _ => currentValue
            };
        }
    }
}