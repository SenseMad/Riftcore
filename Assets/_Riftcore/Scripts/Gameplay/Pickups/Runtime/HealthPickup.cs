using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Players.Core;

namespace Riftcore.Gameplay.Pickups.Runtime
{
    public sealed class HealthPickup : BasePickup
    {
        public override void OnPickup(Player player)
        {
            player.Health.Heal(player.Health.MaxHealth);
            
            base.OnPickup(player);
        }

        protected override bool CanPickup(Player player)
        {
            return player.Health.CurrentHealth < player.Health.MaxHealth;
        }
    }
}