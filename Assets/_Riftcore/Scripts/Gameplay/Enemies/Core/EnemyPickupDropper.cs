using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Gameplay.Pickups.Drops;
using Riftcore.Gameplay.Pickups.Runtime;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyPickupDropper
    {
        private readonly PickupDropService _pickupDropService;

        public EnemyPickupDropper(PickupDropService pickupDropService)
        {
            _pickupDropService = pickupDropService;
        }

        public void DropFromEnemy(Enemy enemy)
        {
            EnemyData enemyData = enemy.EnemyData;
            Vector3 position = enemy.transform.position;

            ExperiencePickup experiencePickup = _pickupDropService.Drop(enemyData.ExperiencePickupPrefab, position) as ExperiencePickup;
            if (experiencePickup != null)
                experiencePickup.SetAmountExperience(enemyData.ExperienceAmount);

            foreach (var pickupDropEntry in enemyData.PickupDropEntries)
            {
                if (pickupDropEntry.PickupPrefab == null)
                    continue;
                
                if (Random.value > pickupDropEntry.Chance)
                    continue;
                
                _pickupDropService.Drop(pickupDropEntry.PickupPrefab, position);
                break;
            }
        }
    }
}