using System.Collections.Generic;
using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.HitHandling.Implementations
{
    public sealed class SingleHitHandler : IProjectileHitHandler
    {
        private Projectile _projectile;
        private ProjectileContext _projectileContext;

        private float _pierce;
        private readonly HashSet<Collider> _hitColliders = new();
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid)
        {
            _projectile = projectile;
            _projectileContext = projectileContext;

            _pierce = projectileContext.PierceCount;
            _hitColliders.Clear();
        }

        public void OnHit(IDamageable damageable, Collider hitCollider)
        {
            if (damageable == null)
                return;
            
            if (!_hitColliders.Add(hitCollider))
                return;
            
            damageable.TakeDamage(_projectileContext.Damage);

            if (_pierce > 0)
            {
                _pierce--;
                return;
            }
            
            _projectile.Die();
        }

        public void Reset()
        {
            _hitColliders.Clear();
            _pierce = _projectileContext.PierceCount;
        }

        public void OnDeath()
        {
            _hitColliders.Clear();
        }
    }
}