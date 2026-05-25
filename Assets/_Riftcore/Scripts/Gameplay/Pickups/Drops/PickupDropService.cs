using Riftcore.Gameplay.Pickups.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Pickups.Drops
{
    public sealed class PickupDropService
    {
        private readonly PickupPool _pickupPool;
        private readonly PickupManager _pickupManager;

        public PickupDropService(PickupPool pickupPool, PickupManager pickupManager)
        {
            _pickupPool = pickupPool;
            _pickupManager = pickupManager;
        }

        public BasePickup Drop(BasePickup pickupPrefab, Vector3 position)
        {
            if (pickupPrefab == null)
                return null;
            
            BasePickup basePickup = _pickupPool.Get(pickupPrefab);
            basePickup.transform.SetPositionAndRotation(position, Quaternion.identity);
            _pickupManager.Register(basePickup);
            
            return basePickup;
        }
    }
}