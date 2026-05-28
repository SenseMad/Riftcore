using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Pickups.Runtime
{
    public sealed class ImmortalityPickup : BasePickup
    {
        [SerializeField, Min(0)] private float _duration = 5.0f;
        
        public override void OnPickup(Player player)
        {
            player.PlayerEffects.ImmortalityEffect.Activate(_duration);
            
            base.OnPickup(player);
        }
    }
}