using System;
using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.Pooling;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class Enemy : PoolItem, IPoolable, IProjectileTarget
    {
        [Inject] private readonly EnemyGrid _enemyGrid;
        [Inject] private readonly EnemyMovement _enemyMovement;
        
        private Rigidbody _rigidbody;
        
        private Vector3 _lastPosition;

        private EnemyVisual _enemyVisual;
        private Collider[] _colliders;
        private bool _isDying;
        
        public Animator Animator { get; private set; }
        
        public EnemyData EnemyData { get; private set; }
        
        public Health Health { get; private set; }
        
        public bool IsActive { get; private set; }

        public Transform Transform => transform;

        public event Action<Enemy> OnDie;

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            
            _rigidbody = GetComponent<Rigidbody>();

            _enemyVisual = GetComponent<EnemyVisual>();
            _colliders = GetComponentsInChildren<Collider>();
            
            Health = GetComponent<Health>();
            Health.OnDied += HandleDied;
        }

        private void OnDestroy()
        {
            if (Health != null)
                Health.OnDied -= HandleDied;
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
            Animator.enabled = !isPaused;
        }
        
        public void OnGetFromPool()
        {
            IsActive = true;
            _isDying = false;
            
            Health.ResetHealth();
            
            _rigidbody.WakeUp();

            _enemyVisual.ResetVisual();
            _enemyVisual.PlaySpawn();

            _lastPosition = transform.position;
        }

        public void OnReturnToPool()
        {
            IsActive = false;
            _isDying = false;
            
            _enemyVisual.ResetVisual();
            
            _rigidbody.Sleep();
        }

        public void PlayDeathAnimation(Action onComplete)
        {
            if (_isDying)
                return;

            _isDying = true;
            IsActive = false;
            
            _rigidbody.Sleep();
            _enemyVisual.PlayDeath(onComplete);
        }
        
        public void ResetSpawnPosition()
        {
            _lastPosition = _rigidbody.position;
        }
        
        public void TakeDamage(float damage)
        {
            if (Health == null)
                return;

            Health.TakeDamage(damage);
        }
        
        private void HandleDied()
        {
            OnDie?.Invoke(this);
        }
    }
}