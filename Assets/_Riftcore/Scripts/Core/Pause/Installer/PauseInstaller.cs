using Riftcore.Core.GameState;
using Zenject;

namespace Riftcore.Core.Pause.Installer
{
    public sealed class PauseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameplayLockService>().AsSingle();
            Container.Bind<PauseService>().AsSingle();
        }
    }
}