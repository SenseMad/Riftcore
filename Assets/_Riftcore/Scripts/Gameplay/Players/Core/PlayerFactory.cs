using Riftcore.Gameplay.Inventory;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Players.Data;
using Riftcore.Gameplay.Players.PlayerCamera;
using Riftcore.Gameplay.Stats;
using Unity.Cinemachine;
using Zenject;

namespace Riftcore.Gameplay.Players.Core
{
    public sealed class PlayerFactory
    {
        private readonly DiContainer _container;
        
        public PlayerFactory(DiContainer container)
        {
            _container = container;
        }

        public Player Create(PlayerData playerData)
        {
            var gameStatistics = new GameStatistics(playerData.PlayerGameStatistics.BaseStatistics);
            
            var player = _container.InstantiatePrefabForComponent<Player>(playerData.PlayerPrefab);
            
            _container.Inject(player);
            
            var cinemachineCamera = _container.Resolve<CinemachineCamera>();
            
            var playerCameraController = player.GetComponentInChildren<PlayerCameraController>();
            playerCameraController.Initialize(cinemachineCamera);
            
            player.Bind(playerData, gameStatistics);

            foreach (var itemData in playerData.StartItems)
            {
                player.PlayerInventory.AddItem(new InventoryItem(itemData));
            }
            
            return player;
        }
    }
}