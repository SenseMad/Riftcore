using System;
using Riftcore.Gameplay.Enemies.Data;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Spawning
{
    [Serializable]
    public sealed class EnemySpawnEntry
    {
        [field: SerializeField] public EnemyData EnemyData { get; private set; }
        
        [field: Header("Настройки спавна")]
        [field: Tooltip("Базовый шанс спавна")]
        [field: SerializeField, Range(0f, 1f)] public float BaseSpawnChance { get; private set; } = 0.5f;
        [field: Tooltip("Минимальное время до появления")]
        [field: SerializeField, Min(0)] public float MinAppearTime { get; private set; } = 0f;
        [field: Tooltip("Базовый интервал спавна")]
        [field: SerializeField, Min(0)] public float BaseSpawnInterval { get; private set; } = 1f;
        [field: Tooltip("Множитель интервала спавна (кривая)")]
        [field: SerializeField] public AnimationCurve IntervalMultiplierCurve { get; private set; } = AnimationCurve.Linear(0f, 1f, 10f, 0.2f);
        
        [field: Header("Групповой спавн")]
        [field: Tooltip("Шанс группового спавна")]
        [field: SerializeField, Range(0f, 1f)] public float GroupSpawnChance { get; private set; } = 0.5f;
        [field: Tooltip("Размер группы (кривая)")]
        [field: SerializeField] public AnimationCurve GroupSizeCurve { get; private set; } = AnimationCurve.Linear(0f, 1f, 1f, 4f);
        [field: Tooltip("Максимальный размер группы")]
        [field: SerializeField, Min(1)] public int MaxGroupSize { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float GroupRadiusPerEnemy { get; private set; } = 0.5f;
        [field: SerializeField, Min(1)] public float MinGroupRadius { get; private set; } = 1f;
        [field: SerializeField, Min(1)] public float MaxGroupRadius { get; private set; } = 8f;
    }
}