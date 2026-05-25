using System;
using UnityEngine;

namespace Riftcore.Gameplay.Skills.Data
{
    [Serializable]
    public sealed class RarityValue
    {
        [field: SerializeField] public SkillRarity SkillRarity { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}