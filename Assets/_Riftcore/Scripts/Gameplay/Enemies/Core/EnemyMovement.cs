using Riftcore.Core.Game;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyMovement
    {
        private readonly GameContext _gameContext;

        public EnemyMovement(GameContext gameContext)
        {
            _gameContext = gameContext;
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
            
            toPlayer.Normalize();

            Vector3 velocity = rigidbody.linearVelocity;

            Vector3 targetVelocity = toPlayer * enemy.EnemyData.SpeedWalking;

            Vector3 horizontalVelocity = Vector3.MoveTowards(new Vector3(velocity.x, 0, velocity.z), targetVelocity, 
                enemy.EnemyData.Acceleration * deltaTime);

            rigidbody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
            
            /*rigidbody.linearVelocity = new Vector3(
                targetVelocity.x,
                velocity.y,
                targetVelocity.z
            );*/
        }
    }
}