using System;
using System.Collections.Generic;
using System.Linq;
using Riftcore.Gameplay.Inventory.Data;

namespace Riftcore.Gameplay.Inventory.Core
{
    public sealed class PlayerInventory
    {
        private readonly List<InventoryItem> _items = new();

        public IReadOnlyList<InventoryItem> Items => _items;
        
        public event Action<InventoryItem> OnItemAdded;

        public void AddItem(InventoryItem item)
        {
            if (item == null)
                return;
            
            if (_items.Any(x => x.ItemData == item.ItemData))
                return;
            
            _items.Add(item);
            
            OnItemAdded?.Invoke(item);
        }

        public InventoryItem GetItem(ItemData itemData)
        {
            return _items.FirstOrDefault(x => x.ItemData == itemData);
        }

        public List<InventoryItem> GetItemsByCategoty(ItemCategory category)
        {
            return _items
                .Where(x => 
                    x != null && 
                    x.ItemData != null && 
                    x.ItemData.ItemCategory == category)
                .ToList();
        }
        
        public bool HasItem(ItemData itemData)
        {
            return GetItem(itemData) != null;
        }
    }
}