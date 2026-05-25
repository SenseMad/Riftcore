using System.Collections.Generic;
using System.Linq;
using Riftcore.Gameplay.Inventory.Data;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Weapons.Data
{
    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Riftcore/Installers/WeaponDatabase")]
    public sealed class WeaponDatabase : ScriptableObjectInstaller<WeaponDatabase>
    {
        [field: SerializeField] public List<WeaponData> Weapons { get; private set; }
        
        public override void InstallBindings()
        {
            Container.Bind<WeaponDatabase>().FromInstance(this).AsSingle();
        }

        public WeaponData GetWeaponData(ItemData itemData)
        {
            WeaponData weaponData = Weapons.FirstOrDefault(x => 
                x != null && itemData != null && x.ItemData.Id == itemData.Id);

            return weaponData;
        }
    }
}