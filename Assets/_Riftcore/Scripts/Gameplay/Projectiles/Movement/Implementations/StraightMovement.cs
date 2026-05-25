using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Movement.Implementations
{
    public sealed class StraightMovement : IProjectileMovement
    {
        private EnemyGrid _enemyGrid;
        
        private Projectile _projectile;
        private float _speed;
        private Vector3 _direction;
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid)
        {
            _projectile = projectile;
            _speed = projectileContext.Speed;
            _direction = _projectile.transform.forward;
            _enemyGrid = enemyGrid;
        }

        public void Tick(float deltaTime)
        {
            _projectile.transform.position += _direction * _speed * deltaTime;
        }
        
        public void SetTarget(Enemy target)
        {
            
        }

        public void Reset()
        {
            _direction = _projectile.transform.forward;
        }

        public void OnDeath()
        {
            
        }
    }
}