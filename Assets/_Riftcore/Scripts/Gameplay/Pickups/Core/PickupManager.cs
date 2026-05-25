using System.Collections.Generic;
using Riftcore.Core.GameState;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Core
{
    public sealed class PickupManager : MonoBehaviour
    {
        [Inject] private readonly PickupPool _pickupPool;
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private readonly List<BasePickup> _activePickups = new();

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            float deltaTime = Time.deltaTime;
            
            for (int i = 0; i < _activePickups.Count; i++)
            {
                var pickup = _activePickups[i];
                if (pickup == null)
                    continue;
                
                pickup.Tick(deltaTime);
            }
        }

        public void Register(BasePickup pickup)
        {
            _activePickups.Add(pickup);
        }

        public void Unregister(BasePickup pickup)
        {
            _activePickups.Remove(pickup);
            
            _pickupPool.Return(pickup);
        }
    }
}