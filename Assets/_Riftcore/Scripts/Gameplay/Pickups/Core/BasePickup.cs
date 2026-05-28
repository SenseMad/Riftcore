using System;
using Riftcore.Gameplay.Effects.Core;
using Riftcore.Gameplay.Pickups.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Pooling;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Pickups.Core
{
    public abstract class BasePickup : PoolItem, IPoolable
    {
        [SerializeField] protected PickupData _pickupData;
        
        [Inject] protected readonly PickupManager _pickupManager;
        [Inject] protected readonly PickupGrid _pickupGrid;
        [Inject] private readonly EffectPool _effectPool;
        
        private Transform _target;
        private bool _isAttracting;
        
        private bool _isPickedUp;
        
        public void Tick(float deltaTime)
        {
            if (!_isAttracting || _target == null)
                return;
            
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _pickupData.AttractSpeed * deltaTime);
            
            _pickupGrid.UpdatePickupCell(this);
        }

        public virtual void OnPickup(Player player)
        {
            _pickupManager.Unregister(this);
        }

        public void OnGetFromPool()
        {
            _isPickedUp = false;
        }

        public void OnReturnToPool()
        {
            _target = null;
            _isAttracting = false;
        }

        public void StartAttract(Transform target)
        {
            if (_isPickedUp)
                return;
            
            _target = target;
            _isAttracting = true;
        }

        protected virtual bool CanPickup(Player player)
        {
            return true;
        }

        protected void PlayPickupEffect()
        {
            if (_pickupData.PickupEffectPrefab == null)
                return;

            Effect effect = _effectPool.Get(_pickupData.PickupEffectPrefab);
            effect.Play(transform.position, Quaternion.identity);
        }

        private void TryPickup(Collider other)
        {
            if (_isPickedUp)
                return;
            
            if (!other.TryGetComponent(out Player player))
                return;
            
            if (!CanPickup(player))
                return;
            
            _isPickedUp = true;

            PlayPickupEffect();

            OnPickup(player);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            TryPickup(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryPickup(other);
        }
    }
}