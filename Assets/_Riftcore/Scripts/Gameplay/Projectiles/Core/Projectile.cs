using Riftcore.Gameplay.Effects.Core;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.Gameplay.Projectiles.Lifetime.Interfaces;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using Riftcore.Pooling;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class Projectile : PoolItem, IPoolable
    {
        [SerializeField] private Effect _hitEffectPrefab;

        [Inject] private readonly EffectPool _effectPool;
        
        private ProjectileManager _projectileManager;

        private ProjectileContext _projectileContext;
        
        private IProjectileMovement _projectileMovement;
        private IProjectileHitHandler _projectileHitHandler;
        private IProjectileLifetime _projectileLifetime;
        
        private Collider _collider;
        private Collider _ownerCollider;
        
        private bool _active;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Tick(float deltaTime)
        {
            if (!_active)
                return;
            
            _projectileLifetime.Tick(deltaTime);
            _projectileMovement.Tick(deltaTime);
        }

        public void Initialize(ProjectileContext projectileContext, ProjectileComponents projectileComponents, EnemyGrid enemyGrid, 
            ProjectileManager projectileManager)
        {
            _projectileContext = projectileContext;
            
            _projectileMovement = projectileComponents.ProjectileMovement;
            _projectileHitHandler = projectileComponents.ProjectileHitHandler;
            _projectileLifetime = projectileComponents.ProjectileLifetime;

            _projectileManager = projectileManager;
            
            _projectileMovement.Initialize(this, projectileContext, enemyGrid);
            _projectileHitHandler.Initialize(this, projectileContext, enemyGrid);
            _projectileLifetime.Initialize(this, projectileContext);

            _ownerCollider = projectileContext.Owner.GetComponent<Collider>();
            Physics.IgnoreCollision(_collider, _ownerCollider);

            _active = true;
        }

        public void SetTarget(Enemy target)
        {
            _projectileMovement?.SetTarget(target);
        }

        public void OnGetFromPool()
        {
            _active = false;
            
            _projectileMovement?.Reset();
            _projectileHitHandler?.Reset();
            _projectileLifetime?.Reset();
        }

        public void OnReturnToPool()
        {
            _projectileMovement?.OnDeath();
            _projectileHitHandler?.OnDeath();
            _projectileLifetime?.OnDeath();
            
            if (_collider != null && _ownerCollider != null)
                Physics.IgnoreCollision(_collider, _ownerCollider, false);
            _ownerCollider = null;
            
            gameObject.SetActive(false);
        }

        public void Die()
        {
            if (!_active)
                return;
            
            _active = false;
            
            _projectileManager.Unregister(this);
        }
        
        private void UseHitEffect()
        {
            if (_hitEffectPrefab == null)
                return;

            var effect = _effectPool.Get(_hitEffectPrefab);
            effect.Play(transform.position, Quaternion.identity);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_active)
                return;
            
            if (_ownerCollider != null && other.transform.root.gameObject == _ownerCollider.gameObject)
                return;
            
            if (!other.TryGetComponent(out IDamageable damageable))
                return;
            
            UseHitEffect();
            
            _projectileHitHandler.OnHit(damageable, other);
        }
    }
}