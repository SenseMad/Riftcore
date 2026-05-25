using Riftcore.World.Grid;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class ProjectileFactory
    {
        private readonly ProjectilePool _projectilePool;
        private readonly EnemyGrid _enemyGrid;

        public ProjectileFactory(ProjectilePool projectilePool, EnemyGrid enemyGrid)
        {
            _projectilePool = projectilePool;
            _enemyGrid = enemyGrid;
        }
        
        /*public Projectile Create(ProjectileWeapon projectileWeapon, Vector3 position, Vector3 direction)
        {
            var context = projectileWeapon.CreateProjectileContext();
            var movement = projectileWeapon.CreateMovement();
            
            var projectileBehaviour = new ProjectileBehaviour
            {
                ProjectileMovement = movement,
                ProjectileHitHandler = null,
                ProjectileLifetime = null
            };

            var projectile = _projectilePool.Get(projectileWeapon.WeaponData);
        }*/
    }
}