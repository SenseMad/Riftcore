using System.Collections.Generic;
using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Players.Core;

namespace Riftcore.Gameplay.Pickups.Runtime
{
    public sealed class MagnetPickup : BasePickup
    {
        public override void OnPickup(Player player)
        {
            var pickups = new List<BasePickup>(_pickupManager.ActivePickups);
            
            foreach (var activePickup in pickups)
            {
                if (activePickup == null)
                    continue;
                
                if (activePickup is not ExperiencePickup experiencePickup)
                    continue;
                
                experiencePickup.StartAttract(player.transform);
            }
            
            base.OnPickup(player);
        }
    }
}