using UnityEngine;

namespace Riftcore.Gameplay.Players.Effects
{
    public sealed class PlayerEffects : MonoBehaviour
    {
        public HealthRegenerationEffect HealthRegenerationEffect { get; private set; }
        public ImmortalityEffect ImmortalityEffect { get; private set; }
        public VampirismEffect VampirismEffect { get; private set; }
        public SpikesEffect SpikesEffect { get; private set; }

        private void Awake()
        {
            HealthRegenerationEffect = GetComponentInChildren<HealthRegenerationEffect>();
            ImmortalityEffect = GetComponentInChildren<ImmortalityEffect>();
            VampirismEffect = GetComponentInChildren<VampirismEffect>();
            SpikesEffect = GetComponentInChildren<SpikesEffect>();
        }
    }
}