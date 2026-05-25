using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.Gameplay.Projectiles.Lifetime.Interfaces;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class ProjectileComponents
    {
        public IProjectileMovement ProjectileMovement;
        public IProjectileHitHandler ProjectileHitHandler;
        public IProjectileLifetime ProjectileLifetime;
    }
}