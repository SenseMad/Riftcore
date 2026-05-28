using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;

namespace Riftcore.Gameplay.Skills.Stats
{
    public sealed class StatValueProvider : IStatValueProvider
    {
        private readonly GameContext _gameContext;

        public StatValueProvider(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
        
        public bool TryGetValue(ItemData itemData, StatType statType, out float value)
        {
            value = 0f;
            
            var player = _gameContext.Player;
            if (player == null)
                return false;
            
            IStatProvider statProvider = GetStatProvider(player, itemData);
            return statProvider != null && statProvider.TryGetStatValue(statType, out value);
        }

        private IStatProvider GetStatProvider(Player player, ItemData itemData)
        {
            if (itemData.ItemCategory == ItemCategory.Weapon)
                return player.PlayerWeaponController.GetWeapon(itemData);

            return player.GameStatistics;
        }
    }
}