using Zenject;

namespace Riftcore.Core.Game.Installer
{
    public sealed class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            
            Container.Bind<GameContext>().AsSingle().NonLazy();
        }
    }
}