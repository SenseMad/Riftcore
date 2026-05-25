using Riftcore.Gameplay.Inventory.Data;

namespace Riftcore.Gameplay.Skills.Stats
{
    public interface IStatValueProvider
    {
        bool TryGetValue(ItemData itemData, StatType statType, out float value);
    }
}