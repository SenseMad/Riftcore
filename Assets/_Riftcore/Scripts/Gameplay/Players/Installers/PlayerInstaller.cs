using Riftcore.Gameplay.Players.Core;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Installer
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineCamera _cinemachineCameraPrefab;
        
        //[Inject] private readonly PlayersInstallerData _playersInstallerData;

        public override void InstallBindings()
        {
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<CinemachineCamera>().FromComponentInNewPrefab(_cinemachineCameraPrefab).AsSingle().NonLazy();
        }
    }
}