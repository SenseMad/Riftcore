using Riftcore.Gameplay.Orbits;
using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Data
{
    [CreateAssetMenu(fileName = "OrbitWeaponData", menuName = "Riftcore/Weapon/Data/OrbitWeaponData")]
    public sealed class OrbitWeaponData : WeaponData
    {
        [field: SerializeField] public OrbitObject OrbitObjectPrefab { get; private set; }
        [field: SerializeField] public OrbitStatistics OrbitStatistics { get; private set; }
        [field: SerializeField] public Vector3 Offset { get; private set; }
    }
}