using System.Collections.Generic;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Players.Core;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Runtime
{
    public sealed class BombPickup : BasePickup
    {
        [SerializeField, Min(0)] private float _radius = 5.0f;
        
        [Inject] private readonly EnemyGrid _enemyGrid;
        
        private readonly List<Enemy> _enemies = new();
        
        public override void OnPickup(Player player)
        {
            _enemies.Clear();
            
            _enemyGrid.GetEnemiesInRadius(transform.position, _radius, _enemies);

            foreach (var enemy in _enemies)
            {
                if (enemy == null)
                    continue;
                
                if (enemy.Health.IsDead)
                    continue;
                
                enemy.TakeDamage(enemy.Health.MaxHealth);
            }
            
            base.OnPickup(player);
        }
    }
}