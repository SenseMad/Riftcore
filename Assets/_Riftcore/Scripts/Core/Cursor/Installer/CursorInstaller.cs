using Zenject;

namespace Riftcore.Core.Cursor.Installer
{
    public sealed class CursorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CursorStateHandler>().AsSingle().NonLazy();
        }
    }
}