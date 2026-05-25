using Riftcore.Gameplay.Weapons.Factories;
using Zenject;

namespace Riftcore.Gameplay.Weapons.Installers
{
    public sealed class WeaponInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WeaponFactory>().AsSingle();
        }
    }
}