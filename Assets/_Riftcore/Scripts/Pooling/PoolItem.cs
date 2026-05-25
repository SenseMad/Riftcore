using UnityEngine;

namespace Riftcore.Pooling
{
    public abstract class PoolItem : MonoBehaviour
    {
        public Component Prefab { get; private set; }

        public void SetPrefab(Component prefab)
        {
            Prefab = prefab;
        }
    }
}