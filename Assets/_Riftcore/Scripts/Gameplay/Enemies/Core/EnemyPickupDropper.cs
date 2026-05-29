using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Gameplay.Enemies.Installers;
using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Pickups.Drops;
using Riftcore.Gameplay.Pickups.Runtime;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyPickupDropper
    {
        private readonly PickupDropService _pickupDropService;
        private readonly EnemyGlobalSpawnInstallerData _enemyGlobalSpawnInstallerData;

        public EnemyPickupDropper(PickupDropService pickupDropService, EnemyGlobalSpawnInstallerData enemyGlobalSpawnInstallerData)
        {
            _pickupDropService = pickupDropService;
            _enemyGlobalSpawnInstallerData = enemyGlobalSpawnInstallerData;
        }

        public void DropFromEnemy(Enemy enemy)
        {
            EnemyData enemyData = enemy.EnemyData;
            Vector3 position = GetDropPosition(enemy);

            ExperiencePickup experiencePickup = _pickupDropService.Drop(enemyData.ExperiencePickupPrefab, position) as ExperiencePickup;
            if (experiencePickup != null)
            {
                experiencePickup.SetAmountExperience(enemyData.ExperienceAmount);
                experiencePickup.transform.position = position + Vector3.up * experiencePickup.Collider.bounds.extents.y;
            }

            foreach (var pickupDropEntry in enemyData.PickupDropEntries)
            {
                if (pickupDropEntry.PickupPrefab == null)
                    continue;
                
                if (Random.value > pickupDropEntry.Chance)
                    continue;
                
                BasePickup pickup = _pickupDropService.Drop(pickupDropEntry.PickupPrefab, position);
                if (pickup != null && pickup.Collider != null)
                    pickup.transform.position = position + Vector3.up * pickup.Collider.bounds.extents.y;
                
                break;
            }
        }
        
        private Vector3 GetDropPosition(Enemy enemy)
        {
            Vector3 position = enemy.transform.position;

            if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 100f, 
                    ~_enemyGlobalSpawnInstallerData.IgnoreClimbMask))
                position = hit.point + Vector3.up * 0.05f;
            
            return position;
        }
    }
}