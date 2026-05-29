using Riftcore.Gameplay.Enemies.Spawning;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Data
{
    [CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "Riftcore/Enemy/SpawnSettings/EnemySpawnSettings")]
    public sealed class EnemySpawnSettings : ScriptableObject
    {
        [field: Header("Сложность")]
        [field: Tooltip("Влияет ли Difficulty на частоту спавна")]
        [field: SerializeField] public bool UseDifficultyForInterval { get; private set; } = true;
        [field: SerializeField, Min(0.05f)] public float MinSpawnInterval { get; private set; } = 0.1f;
        
        [field: Header("Список врагов")]
        [field: SerializeField] public EnemySpawnEntry[] EnemySpawnEntries { get; private set; }
    }
}