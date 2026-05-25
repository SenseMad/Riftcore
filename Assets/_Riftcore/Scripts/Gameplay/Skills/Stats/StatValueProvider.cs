using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory;
using Riftcore.Gameplay.Inventory.Data;

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

            // Тут я тоже думаю надо менять
            if (itemData.ItemCategory == ItemCategory.Weapon)
            {
                var weapon = player.PlayerWeaponController.GetWeapon(itemData);
                return weapon != null && weapon.TryGetStatValue(statType, out value);
            }

            return player.GameStatistics.TryGetValue(statType, out value);
        }
    }
}