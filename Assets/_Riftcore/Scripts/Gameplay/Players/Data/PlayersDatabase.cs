using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Data
{
    [CreateAssetMenu(fileName = "PlayersDatabase", menuName = "Riftcore/Installers/PlayersDatabase")]
    public sealed class PlayersDatabase : ScriptableObjectInstaller<PlayersDatabase>
    {
        [field: SerializeField] public PlayerData[] PlayerDatas { get; private set; }

        public override void InstallBindings()
        {
            Container.Bind<PlayersDatabase>().FromInstance(this).AsSingle();
        }
    }
}