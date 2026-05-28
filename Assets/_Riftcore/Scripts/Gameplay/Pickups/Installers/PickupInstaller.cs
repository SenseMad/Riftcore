using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Pickups.Drops;
using Riftcore.World.Grid;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Installers
{
    public sealed class PickupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PickupGrid>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            
            Container.Bind<PickupPool>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<PickupManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            Container.Bind<PickupDropService>().AsSingle();
        }
    }
}