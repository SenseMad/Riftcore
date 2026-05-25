using Riftcore.Gameplay.Projectiles.Core;

namespace Riftcore.Gameplay.Projectiles.Lifetime.Interfaces
{
    public interface IProjectileLifetime
    {
        void Initialize(Projectile projectile, ProjectileContext projectileContext);
        void Tick(float deltaTime);
        void Reset();
        void OnDeath();
    }
}