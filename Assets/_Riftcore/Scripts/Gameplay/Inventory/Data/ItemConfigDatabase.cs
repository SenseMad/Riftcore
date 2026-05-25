using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemConfigDatabase", menuName = "Riftcore/Installers/ItemConfigDatabase")]
    public sealed class ItemConfigDatabase : ScriptableObjectInstaller<ItemConfigDatabase>
    {
        [field: SerializeField] public List<ItemData> Items { get; private set; }
        
        public override void InstallBindings()
        {
            Container.Bind<ItemConfigDatabase>().FromInstance(this).AsSingle();
        }
    }
}