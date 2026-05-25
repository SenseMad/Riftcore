using Zenject;

namespace Riftcore.Gameplay.Inventory.Installers
{
    public sealed class InventoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<PlayerInventory>().AsSingle();
        }
    }
}