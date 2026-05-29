using Riftcore.Core.Game;
using Riftcore.Gameplay.Enemies.Installers;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyMovement
    {
        private readonly GameContext _gameContext;
        private readonly EnemyGlobalSpawnInstallerData _enemyGlobalSpawnInstallerData;

        public EnemyMovement(GameContext gameContext, EnemyGlobalSpawnInstallerData enemyGlobalSpawnInstallerData)
        {
            _gameContext = gameContext;
            _enemyGlobalSpawnInstallerData = enemyGlobalSpawnInstallerData;
        }

        public void Move(Enemy enemy, Rigidbody rigidbody, float deltaTime)
        {
            if (enemy == null || rigidbody == null || enemy.EnemyData == null)
                return;
            
            var player = _gameContext.Player;
            if (player == null)
                return;
            
            Vector3 toPlayer = player.transform.position - rigidbody.position;
            toPlayer.y = 0f;
            
            if (toPlayer.sqrMagnitude <= 0.001f)
                return;
            
            Vector3 direction = toPlayer.normalized;
            Vector3 currentVelocity = rigidbody.linearVelocity;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            
            bool isWallAhead = IsWallAhead(enemy, rigidbody, direction);
            
            Vector3 targetHorizontalVelocity = direction * enemy.EnemyData.SpeedWalking;
            
            Vector3 horizontalVelocity = Vector3.MoveTowards(
                new Vector3(currentVelocity.x, 0f, currentVelocity.z),
                targetHorizontalVelocity,
                enemy.EnemyData.Acceleration * deltaTime
            );
            
            rigidbody.MoveRotation(Quaternion.RotateTowards(
                rigidbody.rotation,
                targetRotation,
                enemy.EnemyData.RotationSpeed * deltaTime
            ));
            
            float verticalVelocity = currentVelocity.y;
            if (isWallAhead)
            {
                horizontalVelocity *= enemy.EnemyData.ClimbHorizontalMultiplier;
                verticalVelocity = enemy.EnemyData.ClimbSpeed;
            }

            rigidbody.linearVelocity = new Vector3(
                horizontalVelocity.x,
                verticalVelocity,
                horizontalVelocity.z
            );
        }

        private bool IsWallAhead(Enemy enemy, Rigidbody rigidbody, Vector3 direction)
        {
            Vector3 origin = rigidbody.position + Vector3.up * 0.15f;
            
            bool hasHit = Physics.SphereCast(
                origin, 
                enemy.EnemyData.WallCheckRadius, 
                direction, 
                out RaycastHit hit, 
                enemy.EnemyData.WallCheckDistance, 
                ~_enemyGlobalSpawnInstallerData.IgnoreClimbMask, 
                QueryTriggerInteraction.Ignore
                );
            if (!hasHit)
                return false;

            if (hit.collider.attachedRigidbody == rigidbody)
                return false;

            return true;
        }
    }
}