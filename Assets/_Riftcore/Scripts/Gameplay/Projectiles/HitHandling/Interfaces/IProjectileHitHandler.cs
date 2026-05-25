using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.World.Grid;

namespace Riftcore.Gameplay.Projectiles.HitHandling.Interfaces
{
    public interface IProjectileHitHandler
    {
        void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid);
        void OnHit(Enemy enemy);
        void Reset();
        void OnDeath();
    }
}