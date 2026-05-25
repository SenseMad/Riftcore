using Riftcore.Gameplay.Experience.Core;
using Riftcore.Gameplay.Experience.Data;
using Riftcore.Gameplay.Experience.UI;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Experience.Installer
{
    public sealed class ExperienceInstaller : MonoInstaller
    {
        [SerializeField] private ExperienceData _experienceData;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_experienceData).AsSingle();
            
            Container.BindInterfacesAndSelfTo<ExperienceService>().AsSingle();
            
            Container.Bind<ExperienceUI>().FromComponentInHierarchy().AsSingle();
        }
    }
}