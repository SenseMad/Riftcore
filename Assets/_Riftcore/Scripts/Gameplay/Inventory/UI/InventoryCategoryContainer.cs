using System;
using Riftcore.Gameplay.Inventory.Data;
using UnityEngine;

namespace Riftcore.Gameplay.Inventory.UI
{
    [Serializable]
    public sealed class InventoryCategoryContainer
    {
        public Transform Container;
        public ItemCategory Category;
    }
}