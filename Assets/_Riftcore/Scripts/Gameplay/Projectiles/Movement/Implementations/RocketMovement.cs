using System.Collections.Generic;
using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Movement.Implementations
{
    public sealed class RocketMovement : IProjectileMovement
    {
        private EnemyGrid _enemyGrid;

        private readonly List<Enemy> _searchBuffer = new();
        
        private Projectile _projectile;
        private Enemy _target;
        
        private float _speed;
        
        private const float SearchRadius = 25f;
        private const float RetargetDelay = 0.2f;
        
        private float _retargetTimer;
        
        private Vector3 _velocity;
        
        [Header("Launch")]
        private const float LaunchDuration = 0.35f;
        private const float LaunchUpSpeed = 5f;
        private const float LaunchForwardSpeed = 2f;
        private const float LaunchBlendDuration = 0.25f;
        
        private float _launchTimer;
        private Vector3 _launchDirection;
        
        [Header("Homing")]
        private const float TurnSpeed = 5f;
        
        public void Initialize(Projectile projectile, ProjectileContext projectileContext, EnemyGrid enemyGrid)
        {
            _projectile = projectile;
            _enemyGrid = enemyGrid;
            
            _speed = projectileContext.Speed;
            //_target = projectileContext.Target;
            _target = null;

            _retargetTimer = 0f;
            _launchTimer = 0f;
            
            if (_target != null)
            {
                Vector3 flatDirection = (_target.transform.position - _projectile.transform.position);
                flatDirection.y = 0f;
                _launchDirection = flatDirection.normalized;
            }
            else
            {
                _launchDirection = _projectile.transform.forward;
            }
            
            _velocity = Vector3.up * LaunchUpSpeed;
        }
        
        public void Tick(float deltaTime)
        {
            _launchTimer += deltaTime;
            _retargetTimer += deltaTime;
            
            if (_launchTimer < LaunchDuration)
            {
                Vector3 launchVelocity = Vector3.up * LaunchUpSpeed + _launchDirection * LaunchForwardSpeed;

                _velocity = launchVelocity;

                _projectile.transform.position += _velocity * deltaTime;

                RotateToVelocity(deltaTime);

                return;
            }

            if ((_target == null || !_target.IsActive) && _retargetTimer >= RetargetDelay)
            {
                _retargetTimer = 0f;
                Retarget();
            }

            if (_target != null && _target.IsActive)
            {
                Vector3 toTarget = _target.transform.position - _projectile.transform.position;
                if (toTarget.sqrMagnitude > 0.25f)
                {
                    Vector3 desiredVelocity = toTarget.normalized * _speed;

                    float homingBlend =
                        Mathf.Clamp01((_launchTimer - LaunchDuration) / LaunchBlendDuration);

                    _velocity = Vector3.Lerp(
                        _velocity,
                        desiredVelocity,
                        homingBlend * TurnSpeed * deltaTime
                    );
                }
                /*Vector3 desiredVelocity = toTarget.normalized * _speed;
                float homingBlend = Mathf.Clamp01((_launchTimer - LaunchDuration) / LaunchBlendDuration);

                _velocity = Vector3.Lerp(_velocity, desiredVelocity, homingBlend * TurnSpeed * deltaTime);*/
            }
            else
            {
                /*if (_velocity.sqrMagnitude > 0.001f)
                    _velocity = _velocity.normalized * _speed;
                else
                    _velocity = _projectile.transform.forward * _speed;*/
                _target = null;
            }

            _projectile.transform.position += _velocity * deltaTime;
            
            RotateToVelocity(deltaTime);
        }

        private void RotateToVelocity(float deltaTime)
        {
            if (_velocity.sqrMagnitude < 0.001f)
                return;
            
            Quaternion targetRotation = Quaternion.LookRotation(_velocity.normalized);

            _projectile.transform.rotation = Quaternion.Slerp(_projectile.transform.rotation, targetRotation, 10f * deltaTime);
        }

        public void SetTarget(Enemy target)
        {
            _target = target;
        }

        public void Reset()
        {
            _launchTimer = 0f;
            _retargetTimer = 0f;

            _target = null;

            _velocity = Vector3.zero;

            _searchBuffer.Clear();
        }

        public void OnDeath()
        {
            _target = null;
            _searchBuffer.Clear();
        }

        private void Retarget()
        {
            _searchBuffer.Clear();
            
            _enemyGrid.GetEnemiesInRadius(_projectile.transform.position, SearchRadius, _searchBuffer);
            /*if (_searchBuffer.Count == 0)
            {
                _target = null;
                return;
            }*/

            Enemy bestEnemy = null;
            float bestDistance = float.MaxValue;
            
            Vector3 projectilePosition = _projectile.transform.position;

            for (int i = 0; i < _searchBuffer.Count; i++)
            {
                Enemy enemy = _searchBuffer[i];
                if (enemy == null)
                    continue;
                
                float sqrDistance = (enemy.transform.position - projectilePosition).sqrMagnitude;
                if (sqrDistance < bestDistance)
                {
                    bestDistance = sqrDistance;
                    bestEnemy = enemy;
                }
            }

            _target = bestEnemy;
        }
    }
}