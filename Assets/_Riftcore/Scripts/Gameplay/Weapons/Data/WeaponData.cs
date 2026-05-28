using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Riftcore/Weapon/Data/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field: Header("Data")]
        [field: SerializeField] public ItemData ItemData { get; private set; }
        
        [field: Header("Attack")]
        [field: SerializeField, Min(0)] public float AttackRange { get; private set; }
        [field: SerializeField, Min(0)] public float AttackPerMinutes { get; private set; }
        
        [field: Header("Statistics")]
        [field: SerializeField] public CombatStatistics CombatStatistics { get; private set; }
    }
}