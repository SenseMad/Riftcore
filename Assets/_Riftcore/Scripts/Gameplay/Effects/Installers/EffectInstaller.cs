using Riftcore.Gameplay.Effects.Core;
using Zenject;

namespace Riftcore.Gameplay.Effects.Installers
{
    public sealed class EffectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EffectPool>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}