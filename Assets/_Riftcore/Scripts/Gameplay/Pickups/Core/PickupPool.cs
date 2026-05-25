using Riftcore.Pooling;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Core
{
    public sealed class PickupPool : MonoBehaviour
    {
        private ComponentPool<BasePickup> _pool;

        [Inject]
        public void Construct(DiContainer container)
        {
            _pool = new ComponentPool<BasePickup>(container, transform);
        }

        public BasePickup Get(BasePickup prefab)
        {
            return _pool.Get(prefab);
        }

        public void Return(BasePickup pickup)
        {
            _pool.Return(pickup);
        }
    }
}