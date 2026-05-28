using System.Collections.Generic;
using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.HitHandling.Implementations
{
    public sealed class BounceHitHandler : IProjectileHitHandler
    {
        private Projectile _projectile;
        private ProjectileContext _projectileContext;
        private EnemyGrid _enemyGrid;

        private int _bounce;
        private float _bounceRadius;
        
        private readonly HashSet<Enemy> _hitEnemies = new();
        private readonly List<Enemy> _enemiesBuffer = new();
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid)
        {
            _projectile = projectile;
            _projectileContext = projectileContext;
            _enemyGrid = enemyGrid;

            _bounce = projectileContext.BounceCount;
            _bounceRadius = projectileContext.BounceRadius;
            _hitEnemies.Clear();
        }

        public void OnHit(IDamageable damageable, Collider hitCollider)
        {
            if (damageable == null)
                return;

            if (!hitCollider.TryGetComponent(out Enemy enemy))
            {
                damageable.TakeDamage(_projectileContext.Damage);
                _projectile.Die();
                return;
            }
            
            if (!_hitEnemies.Add(enemy))
                return;
            
            enemy.TakeDamage(_projectileContext.Damage);

            if (_bounce <= 0)
            {
                _projectile.Die();
                return;
            }

            Enemy nextEnemy = FindNextEnemy(enemy);
            if (nextEnemy == null)
            {
                _projectile.Die();
                return;
            }

            _bounce--;
            _projectile.SetTarget(nextEnemy);
        }

        public void Reset()
        {
            _bounce = _projectileContext.BounceCount;
            _hitEnemies.Clear();
        }

        public void OnDeath()
        {
            _hitEnemies.Clear();
        }

        private Enemy FindNextEnemy(Enemy currentEnemy)
        {
            _enemiesBuffer.Clear();

            Vector3 searchPosition = currentEnemy.transform.position;
            
            _enemyGrid.GetEnemiesInRadius(searchPosition, _bounceRadius, _enemiesBuffer);

            _enemiesBuffer.RemoveAll(enemy => enemy == null || _hitEnemies.Contains(enemy));

            Enemy bestEnemy = null;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < _enemiesBuffer.Count; i++)
            {
                Enemy enemy = _enemiesBuffer[i];
                if (enemy == null)
                    continue;
                
                float sqrDistance = (enemy.transform.position - searchPosition).sqrMagnitude;
                if (sqrDistance < bestDistance)
                {
                    bestDistance = sqrDistance;
                    bestEnemy = enemy;
                }
            }

            return bestEnemy;
        }
    }
}