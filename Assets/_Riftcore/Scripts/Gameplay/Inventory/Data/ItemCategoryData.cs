using UnityEngine;

namespace Riftcore.Gameplay.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemCategoryData", menuName = "Riftcore/Inventory/Data/ItemCategoryData")]
    public sealed class ItemCategoryData : ScriptableObject
    {
        [field: SerializeField] public ItemCategory ItemCategory { get; private set; }
        [field: Tooltip("Максимальное количество предметов данного типа, которое игрок может носить с собой одновременно")]
        [field: SerializeField, Min(1)] public int MaxOwnedItems { get; private set; }
    }
}