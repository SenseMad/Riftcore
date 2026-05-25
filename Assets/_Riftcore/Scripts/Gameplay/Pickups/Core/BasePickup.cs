using Riftcore.Gameplay.Pickups.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Pickups.Core
{
    public abstract class BasePickup : PoolItem, IPoolable
    {
        [SerializeField] protected PickupData _pickupData;
        
        [Inject] protected readonly PickupManager _pickupManager;
        
        public void Tick(float deltaTime)
        {
            
        }
        
        public abstract void OnPickup(Player player);

        public void OnGetFromPool()
        {
            
        }

        public void OnReturnToPool()
        {
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player player))
                return;

            OnPickup(player);
        }
    }
}