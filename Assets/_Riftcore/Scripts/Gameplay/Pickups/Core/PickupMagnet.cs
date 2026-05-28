using System.Collections.Generic;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Players.Core;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Core
{
    public sealed class PickupMagnet : MonoBehaviour
    {
        [Inject] private readonly PickupGrid _pickupGrid;
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private readonly List<BasePickup> _pickups = new();

        private Player _player;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            _pickupGrid.GetPickupsInRadius(_player.transform.position, _player.GameStatistics.LevelStatistics.CollectingRadius, _pickups);

            for (int i = 0; i < _pickups.Count; i++)
            {
                BasePickup pickup = _pickups[i];
                if (pickup == null)
                    continue;
                
                pickup.StartAttract(transform);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_player == null)
                return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_player.transform.position, _player.GameStatistics.LevelStatistics.CollectingRadius);
        }
    }
}