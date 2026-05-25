using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.World.Grid;

namespace Riftcore.Gameplay.Projectiles.Movement.Interfaces
{
    public interface IProjectileMovement
    {
        void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid);
        void Tick(float deltaTime);
        void SetTarget(Enemy target);
        void Reset();
        void OnDeath();
    }
}