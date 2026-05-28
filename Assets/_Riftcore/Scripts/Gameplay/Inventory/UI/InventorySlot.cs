using Riftcore.Gameplay.Inventory.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Riftcore.Gameplay.Inventory.UI
{
    public sealed class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _levelText;
        
        public InventoryItem InventoryItem { get; private set; }

        private void Awake()
        {
            _icon.gameObject.SetActive(false);
            _levelText.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (InventoryItem != null)
                InventoryItem.OnLevelUp -= OnLevelUp;
        }
        
        public void Initialize(InventoryItem inventoryItem)
        {
            if (InventoryItem != null)
                InventoryItem.OnLevelUp -= OnLevelUp;
            
            InventoryItem = inventoryItem;
            
            InventoryItem.OnLevelUp += OnLevelUp;

            UpdateInventoryItem();
        }
        
        private void OnLevelUp(int level)
        {
            UpdateLevelText(level);
        }

        private void UpdateInventoryItem()
        {
            _icon.gameObject.SetActive(true);
            _levelText.gameObject.SetActive(true);

            _icon.sprite = InventoryItem.ItemData.Icon;
            UpdateLevelText(InventoryItem.Level);
        }

        private void UpdateLevelText(int level)
        {
            _levelText.text = $"LVL {level}";
        }
    }
}