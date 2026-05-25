using Riftcore.Gameplay.Projectiles.Core;
using Zenject;

namespace Riftcore.Gameplay.Projectiles.Installer
{
    public sealed class ProjectileInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProjectilePool>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<ProjectileManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}