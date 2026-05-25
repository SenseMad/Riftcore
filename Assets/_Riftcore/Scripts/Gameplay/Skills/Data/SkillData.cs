using System.Collections.Generic;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Skills.Data
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Riftcore/SkillSystem/Data/SkillData")]
    public sealed class SkillData : ScriptableObject
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public StatOperation StatOperation { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public bool IsPercentValue { get; private set; }
        
        [field: SerializeField] public List<RarityValue> RarityValues { get; private set; }

        public StatModifierData GetModifier(SkillRarity skillRarity)
        {
            float value = GetValue(skillRarity);

            return new StatModifierData(StatType, StatOperation, value);
        }

        public float GetValue(SkillRarity skillRarity)
        {
            foreach (var rarityValue in RarityValues)
            {
                if (rarityValue.SkillRarity == skillRarity)
                    return rarityValue.Value;
            }

            return 0f;
        }

        public string GetFormattedChange(float oldValue, SkillRarity skillRarity)
        {
            var modifier = GetModifier(skillRarity);
            float newValue = modifier.Apply(oldValue);

            return $"{Name}: {FormatValue(oldValue)} -> {FormatValue(newValue)}";
        }

        private string FormatValue(float value)
        {
            return IsPercentValue ? $"{value:0.#}%" : $"{value:0.#}";
        }
    }
}