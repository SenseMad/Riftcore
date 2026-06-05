using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Level.Installers
{
    public sealed class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameRunStatistics>().AsSingle().NonLazy();
        }
    }
}