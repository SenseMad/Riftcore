using Riftcore.Pooling;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyPool : MonoBehaviour
    {
        private ComponentPool<Enemy> _pool;

        [Inject]
        public void Construct(DiContainer container)
        {
            _pool = new ComponentPool<Enemy>(container, transform);
        }

        public Enemy Get(Enemy prefab)
        {
            return _pool.Get(prefab);
        }

        public void Return(Enemy enemy)
        {
            _pool.Return(enemy);
        }
    }
}