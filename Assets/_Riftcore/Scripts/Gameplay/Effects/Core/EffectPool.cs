using Riftcore.Pooling;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Effects.Core
{
    public sealed class EffectPool : MonoBehaviour
    {
        private ComponentPool<HitEffect> _pool;

        [Inject]
        public void Construct(DiContainer container)
        {
            _pool = new ComponentPool<HitEffect>(container, transform);
        }

        public HitEffect Get(HitEffect prefab)
        {
            return _pool.Get(prefab);
        }

        public void Return(HitEffect hitEffect)
        {
            _pool.Return(hitEffect);
        }
    }
}