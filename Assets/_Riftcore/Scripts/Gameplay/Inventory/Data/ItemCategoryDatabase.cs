using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemCategoryDatabase", menuName = "Riftcore/Installers/ItemCategoryDatabase")]
    public sealed class ItemCategoryDatabase : ScriptableObjectInstaller<ItemCategoryDatabase>
    {
        [field: SerializeField] public List<ItemCategoryData> Categories { get; private set; }
        
        public override void InstallBindings()
        {
            Container.Bind<ItemCategoryDatabase>().FromInstance(this).AsSingle();
        }

        public int GetMaxItems(ItemCategory category)
        {
            foreach (var categoryData in Categories)
            {
                if (categoryData.ItemCategory == category)
                    return categoryData.MaxOwnedItems;
            }

            return int.MaxValue;
        }
    }
}