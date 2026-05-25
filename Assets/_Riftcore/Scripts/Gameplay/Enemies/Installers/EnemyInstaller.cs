using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Gameplay.Enemies.Spawning;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Installers
{
    public sealed class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawnSettings _enemySpawnSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<EnemyPool>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            Container.Bind<EnemyMovement>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.Bind<EnemySpawnSettings>().FromInstance(_enemySpawnSettings).AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemySpawnPointFinder>().AsSingle();
            
            Container.Bind<EnemyManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<EnemySpawnManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<EnemyGrid>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            Container.Bind<EnemyPickupDropper>().AsSingle();
        }
    }
}