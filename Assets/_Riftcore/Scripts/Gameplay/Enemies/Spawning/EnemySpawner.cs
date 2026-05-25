using Riftcore.Gameplay.Enemies.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Spawning
{
    public sealed class EnemySpawner
    {
        private readonly EnemyPool _enemyPool;

        public EnemySpawner(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
        }
        
        public Enemy SpawnEnemy(Enemy enemyPrefab, Vector3 position)
        {
            var enemy = _enemyPool.Get(enemyPrefab);
            
            enemy.gameObject.SetActive(false);
            enemy.transform.SetPositionAndRotation(position, Quaternion.identity);
            enemy.gameObject.SetActive(true);
            
            return enemy;
        }

        public void Despawn(Enemy enemy)
        {
            if (enemy == null)
                return;
            
            _enemyPool.Return(enemy);
        }
    }
}