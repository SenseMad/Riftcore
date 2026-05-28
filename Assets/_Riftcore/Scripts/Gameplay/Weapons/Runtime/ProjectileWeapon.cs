using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.HitHandling.Implementations;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.Gameplay.Projectiles.Lifetime.Implementations;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using Riftcore.Gameplay.Skills.Stats;
using Riftcore.Gameplay.Stats;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Runtime
{
    public sealed class ProjectileWeapon : Weapon<ProjectileWeaponData>
    {
        private readonly ProjectileManager _projectileManager;
        private readonly ProjectilePool _projectilePool;

        public ProjectileStatistics ProjectileStatistics { get; }

        public ProjectileWeapon(ProjectileWeaponData weaponData, Player player, 
            ProjectileManager projectileManager, ProjectilePool projectilePool, EnemyGrid enemyGrid) 
            : base(weaponData, player, enemyGrid)
        {
            ProjectileStatistics = new ProjectileStatistics(_weaponData.ProjectileStatistics);

            _projectileManager = projectileManager;
            _projectilePool = projectilePool;
        }

        public override void ApplyModifier(StatModifierData statModifierData)
        {
            base.ApplyModifier(statModifierData);
            
            ProjectileStatistics.ApplyModifier(statModifierData);
        }

        public override bool TryGetStatValue(StatType statType, out float value)
        {
            if (base.TryGetStatValue(statType, out value))
                return true;
            
            return ProjectileStatistics.TryGetValue(statType, out value);
        }

        protected override void Attack(Enemy target)
        {
            int projectileCount = (int)(ProjectileStatistics.ProjectileCount +
                                  _player.GameStatistics.ProjectileStatistics.ProjectileCount);

            for (int i = 0; i < projectileCount; i++)
            {
                SpawnProjectile(target);
            }
        }

        private void SpawnProjectile(Enemy target)
        {
            Projectile projectile = _projectilePool.Get(_weaponData.ProjectilePrefab);

            var spawnPosition = _player.transform.position;
            spawnPosition.y = _player.Collider.bounds.center.y;
            
            projectile.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            
            _projectileManager.Register(projectile);

            ProjectileContext projectileContext = CreateProjectileContext();

            var projectileBehaviour = new ProjectileComponents
            {
                ProjectileMovement = CreateMovement(),
                ProjectileHitHandler = CreateProjectileHitHandler(),
                ProjectileLifetime = new DurationProjectileLifetime()
            };
            
            projectile.Initialize(projectileContext, projectileBehaviour, _enemyGrid, _projectileManager);
            projectile.SetTarget(target);
        }

        public ProjectileContext CreateProjectileContext()
        {
            int finalDamage = CriticalDamage(out bool isCritical);
            float projectileRange = ProjectileStatistics.ProjectileFiringRange;
            float projectileSize = ProjectileStatistics.ProjectileSize * _player.GameStatistics.ProjectileStatistics.ProjectileSize;
            float projectileSpeed = ProjectileStatistics.ProjectileSpeed * _player.GameStatistics.ProjectileStatistics.ProjectileSpeed;
            float duration = ProjectileStatistics.Duration * _player.GameStatistics.ProjectileStatistics.Duration;

            int bounceCount = (int)(ProjectileStatistics.ProjectileBounceCount + _player.GameStatistics.ProjectileStatistics.ProjectileBounceCount);
            int pierceCount = (int)(ProjectileStatistics.ProjectilePenetration + _player.GameStatistics.ProjectileStatistics.ProjectilePenetration);
            
            var projectileContext = new ProjectileContext
            {
                Owner = _player.gameObject,
                Damage = finalDamage,
                FiringRange = projectileRange,
                Speed = projectileSpeed,
                Size = projectileSize,
                Duration = duration,
                IsCritical = isCritical,
                BounceCount = bounceCount,
                BounceRadius = 15,
                PierceCount = pierceCount,
            };

            return projectileContext;
        }

        private IProjectileMovement CreateMovement()
        {
            return _weaponData.ProjectileMovementFactory.Create();
        }

        private IProjectileHitHandler CreateProjectileHitHandler()
        {
            if (ProjectileStatistics.ProjectileBounceCount +
                _player.GameStatistics.ProjectileStatistics.ProjectileBounceCount > 0)
                return new BounceHitHandler();

            return new SingleHitHandler();
        }
    }
}