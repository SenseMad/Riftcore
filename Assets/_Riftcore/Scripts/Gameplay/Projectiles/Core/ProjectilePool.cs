using Riftcore.Pooling;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class ProjectilePool : MonoBehaviour
    {
        private ComponentPool<Projectile> _pool;

        [Inject]
        public void Construct(DiContainer container)
        {
            _pool = new ComponentPool<Projectile>(container, transform);
        }

        public Projectile Get(Projectile prefab)
        {
            return _pool.Get(prefab);
        }

        public void Return(Projectile projectile)
        {
            _pool.Return(projectile);
        }
    }
}