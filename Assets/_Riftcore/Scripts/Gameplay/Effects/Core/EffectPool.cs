using Riftcore.Pooling;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Effects.Core
{
    public sealed class EffectPool : MonoBehaviour
    {
        private ComponentPool<Effect> _pool;

        [Inject]
        public void Construct(DiContainer container)
        {
            _pool = new ComponentPool<Effect>(container, transform);
        }

        public Effect Get(Effect prefab)
        {
            return _pool.Get(prefab);
        }

        public void Return(Effect effect)
        {
            _pool.Return(effect);
        }
    }
}