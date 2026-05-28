using System.Collections.Generic;
using Riftcore.Core.Game;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Inventory.UI
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlot _inventorySlotPrefab;
        
        [SerializeField] private List<InventoryCategoryContainer> _categoryContainers;
        
        private readonly Dictionary<ItemCategory, List<InventorySlot>> _inventorySlots = new();
        
        [Inject] private readonly GameContext _gameContext;

        private Player _player;

        private void Awake()
        {
            _gameContext.OnPlayerSpawned += OnPlayerSpawned;
            
            if (_gameContext.Player != null)
                OnPlayerSpawned(_gameContext.Player);
        }
        
        private void OnDestroy()
        {
            _gameContext.OnPlayerSpawned -= OnPlayerSpawned;
            
            if (_player != null)
                _player.PlayerInventory.OnItemAdded -= OnItemAdded;
        }

        private void CreateSlotsForCategory(ItemCategory category, int slotCount)
        {
            if (_inventorySlots.ContainsKey(category))
                return;
                
            InventoryCategoryContainer categoryContainer = null;
            foreach (var container in _categoryContainers)
            {
                if (container.Category != category)
                    continue;
                
                categoryContainer = container;
                break;
            }
            
            if (categoryContainer == null)
                return;
            
            var slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                InventorySlot inventorySlot = Instantiate(_inventorySlotPrefab, categoryContainer.Container);
                slots.Add(inventorySlot);
            }
            
            _inventorySlots[category] = slots;
        }
        
        private void OnPlayerSpawned(Player player)
        {
            if (_player != null)
                _player.PlayerInventory.OnItemAdded -= OnItemAdded;
            
            _player = player;
            _player.PlayerInventory.OnItemAdded += OnItemAdded;
            
            CreateSlotsForCategory(ItemCategory.Weapon, 3);
            CreateSlotsForCategory(ItemCategory.Passive, 3);

            foreach (var item in _player.PlayerInventory.Items)
                OnItemAdded(item);
        }

        private void OnItemAdded(InventoryItem inventoryItem)
        {
            if (!_inventorySlots.TryGetValue(inventoryItem.ItemData.ItemCategory, out var slots))
                return;

            foreach (var slot in slots)
            {
                if (slot == null || slot.InventoryItem != null)
                    continue;

                slot.Initialize(inventoryItem);
                return;
            }
        }
    }
}