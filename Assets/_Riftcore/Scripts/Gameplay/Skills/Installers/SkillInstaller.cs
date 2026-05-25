using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Skills.Data;
using Riftcore.Gameplay.Skills.Runtime;
using Riftcore.Gameplay.Skills.Stats;
using Riftcore.Gameplay.Skills.UI;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Skills.Installers
{
    public sealed class SkillInstaller : MonoInstaller
    {
        [SerializeField] private SkillChoicePanel _skillChoicePanelPrefab;
        
        [Inject] private readonly ItemConfigDatabase _itemConfigDatabase;
        [Inject] private readonly RarityDatabase _rarityDatabase;
        
        public override void InstallBindings()
        {
            Container.Bind<SkillSelectionService>().AsSingle();
            Container.Bind<SkillRewardApplier>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatValueProvider>().AsSingle();

            Container.Bind<ISkillChoicePanel>().FromInstance(_skillChoicePanelPrefab).AsSingle();
            //Container.Bind<SkillChoicePanel>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SkillDropSystem>().AsSingle().NonLazy();
        }
    }
}