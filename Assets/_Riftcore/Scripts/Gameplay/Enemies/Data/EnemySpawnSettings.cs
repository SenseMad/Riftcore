using Riftcore.Gameplay.Enemies.Spawning;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Data
{
    [CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "Riftcore/Enemy/SpawnSettings/EnemySpawnSettings")]
    public sealed class EnemySpawnSettings : ScriptableObject
    {
        [field: SerializeField] public EnemySpawnEntry[] EnemySpawnEntries { get; private set; }
    }
}