using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Installers
{
    [CreateAssetMenu(fileName = "EnemyGlobalSpawnInstallerData", menuName = "Riftcore/Installers/EnemyGlobalSpawnInstallerData")]
    public sealed class EnemyGlobalSpawnInstallerData : ScriptableObjectInstaller<EnemyGlobalSpawnInstallerData>
    {
        [field: SerializeField, Min(0)] public float MinSpawnDistance { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float MaxSpawnDistance { get; private set; } = 25f;
        [field: SerializeField, Min(0)] public float SpawnAngle { get; private set; } = 75f;
        [field: SerializeField, Min(0)] public float SpawnHeight { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float CheckRadius { get; private set; } = 0.5f;
        
        [field: SerializeField] public LayerMask GroundMask { get; private set; }
        
        public override void InstallBindings()
        {
            Container.Bind<EnemyGlobalSpawnInstallerData>().FromInstance(this).AsSingle();
        }
    }
}