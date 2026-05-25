using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.Lifetime.Interfaces;

namespace Riftcore.Gameplay.Projectiles.Lifetime.Implementations
{
    public sealed class DurationProjectileLifetime : IProjectileLifetime
    {
        private Projectile _projectile;
        private float _time;
        private float _duration;
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext)
        {
            _projectile = projectile;
            _duration = projectileContext.Duration;
            _time = 0;
        }

        public void Tick(float deltaTime)
        {
            _time += deltaTime;
            
            if (_time >= _duration)
                _projectile.Die();
        }

        public void Reset()
        {
            _time = 0;
        }

        public void OnDeath()
        {
            _time = 0;
        }
    }
}