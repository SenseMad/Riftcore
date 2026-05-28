using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Projectiles.Core;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.Gameplay.Weapons.Runtime;
using Riftcore.World.Grid;
using Zenject;

namespace Riftcore.Gameplay.Weapons.Factories
{
    public sealed class WeaponFactory
    {
        [Inject] private readonly ProjectileManager _projectileManager;
        [Inject] private readonly ProjectilePool _projectilePool;
        [Inject] private readonly EnemyGrid _enemyGrid;

        public Weapon Create(WeaponData weaponData, Player player)
        {
            return weaponData switch
            {
                ProjectileWeaponData projectileWeaponData => new ProjectileWeapon(projectileWeaponData, player, _projectileManager, _projectilePool, _enemyGrid),
                WeaponData auraWeapon => new AuraWeapon(weaponData, player, _enemyGrid),
                
                _ => null
            };
        }
    }
}