using System;
using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Pooling;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class Enemy : PoolItem, IPoolable
    {
        [Inject] private readonly EnemyGrid _enemyGrid;
        [Inject] private readonly EnemyMovement _enemyMovement;
        
        private Rigidbody _rigidbody;
        
        private Vector3 _lastPosition;
        
        public EnemyData EnemyData { get; private set; }
        
        public bool IsActive { get; private set; }

        public event Action<Enemy> OnDie;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Tick(float deltaTime)
        {
            _enemyMovement.Move(this, _rigidbody, deltaTime);
            
            Vector3 currentPosition = _rigidbody.position;

            if ((currentPosition - _lastPosition).sqrMagnitude > 0.25f)
            {
                _enemyGrid.UpdateEnemyCell(this);
                _lastPosition = currentPosition;
            }
        }

        public void Initialize(EnemyData enemyData)
        {
            EnemyData = enemyData;
        }
        
        public void SetPaused(bool isPaused)
        {
            _rigidbody.isKinematic = isPaused;
        }
        
        public void OnGetFromPool()
        {
            IsActive = true;
            
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.WakeUp();

            _lastPosition = transform.position;
        }

        public void OnReturnToPool()
        {
            IsActive = false;
            
            //_enemyGrid.Unregister(this);
            
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.Sleep();
        }
        
        public void ResetSpawnPosition()
        {
            _lastPosition = _rigidbody.position;
        }

        public void TakeDamage(float damage)
        {
            OnDie?.Invoke(this);
        }
    }
}