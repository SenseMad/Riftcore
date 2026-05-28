using System;
using Riftcore.Gameplay.Inventory.Data;

namespace Riftcore.Gameplay.Inventory.Core
{
    public sealed class InventoryItem
    {
        public ItemData ItemData { get; }
        public int Level { get; private set; }

        public bool IsMaxLevel => Level >= ItemData.MaxLevel;
        
        public event Action<int> OnLevelUp;
        
        public InventoryItem(ItemData itemData)
        {
            ItemData = itemData;
            Level = 1;
        }

        public void LevelUp()
        {
            if (IsMaxLevel)
                return;
            
            Level++;
            OnLevelUp?.Invoke(Level);
        }
    }
}