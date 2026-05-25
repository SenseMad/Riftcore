using System;
using UnityEngine;

namespace Riftcore.Gameplay.Skills.Data
{
    [Serializable]
    public sealed class RarityChanceData
    {
        [field: SerializeField] public SkillRarity SkillRarity { get; private set; }
        [field: SerializeField, Min(0)] public float Weight { get; private set; }
        
        [field: Header("Visual")]
        [field: SerializeField] public Color Color { get; private set; }
    }
}