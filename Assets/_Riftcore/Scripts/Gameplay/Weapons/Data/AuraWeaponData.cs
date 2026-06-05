using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Data
{
    [CreateAssetMenu(fileName = "AuraWeaponData", menuName = "Riftcore/Weapon/Data/AuraWeaponData")]
    public sealed class AuraWeaponData : WeaponData
    {
        [field: SerializeField] public GameObject EffectPrefab { get; private set; }
    }
}