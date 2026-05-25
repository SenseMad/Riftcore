using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Riftcore.Pooling
{
    public sealed class ComponentPool<T> where T : PoolItem
    {
        private readonly Dictionary<T, Queue<T>> _pools = new();
        private readonly DiContainer _container;
        private readonly Transform _root;
        
        public ComponentPool(DiContainer container, Transform root)
        {
            _container = container;
            _root = root;
        }

        public T Get(T prefab)
        {
            if (!_pools.TryGetValue(prefab, out Queue<T> pool))
            {
                pool = new Queue<T>();
                _pools[prefab] = pool;
            }

            T item;

            if (pool.Count > 0)
                item = pool.Dequeue();
            else
            {
                item = _container.InstantiatePrefabForComponent<T>(prefab, _root);
                item.SetPrefab(prefab);
            }
            
            item.transform.SetParent(_root);
            item.gameObject.SetActive(true);
            
            if (item.TryGetComponent(out IPoolable poolable))
                poolable.OnGetFromPool();

            return item;
        }

        public void Return(T item)
        {
            if (item == null)
                return;
            
            if (item.TryGetComponent(out IPoolable poolable))
                poolable.OnReturnToPool();
            
            item.gameObject.SetActive(false);
            item.transform.SetParent(_root);

            T prefab = (T)item.Prefab;

            if (!_pools.TryGetValue(prefab, out Queue<T> pool))
            {
                pool = new Queue<T>();
                _pools[prefab] = pool;
            }
            
            pool.Enqueue(item);
        }

        public void Clear()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    var item = pool.Dequeue();
                    if (item == null)
                        continue;
                    
                    Object.Destroy(item.gameObject);
                }
            }
            
            _pools.Clear();
        }
    }
}