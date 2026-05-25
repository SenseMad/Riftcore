using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Projectiles.Movement.Factories;
using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Data
{
    [CreateAssetMenu(fileName = "ProjectileWeaponData", menuName = "Riftcore/Weapon/Data/ProjectileWeaponData")]
    public sealed class ProjectileWeaponData : WeaponData
    {
        [field: SerializeField] public ProjectileStatistics ProjectileStatistics { get; private set; }
        
        [field: Header("Projectile")]
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
        
        [field: Header("Movement")]
        [field: SerializeField] public ProjectileMovementFactory ProjectileMovementFactory { get; private set; }
    }
}