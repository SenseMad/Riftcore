using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.HitHandling.Interfaces
{
    public interface IProjectileHitHandler
    {
        void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid);
        void OnHit(IDamageable damageable, Collider hitCollider);
        void Reset();
        void OnDeath();
    }
}