using System.Collections.Generic;
using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.World.Grid;

namespace Riftcore.Gameplay.Projectiles.HitHandling.Implementations
{
    public sealed class SingleHitHandler : IProjectileHitHandler
    {
        private Projectile _projectile;
        private ProjectileContext _projectileContext;

        private float _pierce;
        private readonly HashSet<Enemy> _hitEnemies = new();
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid)
        {
            _projectile = projectile;
            _projectileContext = projectileContext;

            _pierce = projectileContext.PierceCount;
            _hitEnemies.Clear();
        }

        public void OnHit(Enemy enemy)
        {
            if (!_hitEnemies.Add(enemy))
                return;
            
            enemy.TakeDamage(_projectileContext.Damage);

            if (_pierce > 0)
            {
                _pierce--;
                return;
            }
            
            _projectile.Die();
        }

        public void Reset()
        {
            _hitEnemies.Clear();
            _pierce = _projectileContext.PierceCount;
        }

        public void OnDeath()
        {
            _hitEnemies.Clear();
        }
    }
}