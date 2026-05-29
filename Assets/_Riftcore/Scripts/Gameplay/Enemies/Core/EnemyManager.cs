using System.Collections.Generic;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Enemies.Spawning;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [Inject] private readonly EnemyGrid _enemyGrid;
        [Inject] private readonly EnemySpawner _enemySpawner;
        [Inject] private readonly EnemyPickupDropper _enemyPickupDropper;
        [Inject] private readonly GameplayLockService _gameplayLockService;

        private const float UpdateTime = 0.1f;
        private float _time;
        
        private readonly List<Enemy> _enemies = new();

        private void OnEnable()
        {
            _gameplayLockService.OnGameplayAllowedChanged += OnGameplayAllowedChanged;
        }

        private void OnDisable()
        {
            _gameplayLockService.OnGameplayAllowedChanged -= OnGameplayAllowedChanged;
        }

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            if (Time.time - _time < UpdateTime)
                return;
            
            _time = Time.time;
            
            for (int i = 0; i < _enemies.Count; i++)
            {
                var enemy = _enemies[i];
                if (enemy == null)
                    continue;
                
                enemy.Tick(UpdateTime);
            }
        }

        public void Register(Enemy enemy)
        {
            if (enemy == null)
                return;
            
            if (_enemies.Contains(enemy))
                return;
            
            _enemies.Add(enemy);
            
            enemy.OnDie += OnEnemyDie;
            
            _enemyGrid.Register(enemy);
        }

        public void Unregister(Enemy enemy)
        {
            if (enemy == null)
                return;
            
            if (!_enemies.Remove(enemy))
                return;
            
            enemy.OnDie -= OnEnemyDie;
            
            _enemyGrid.Unregister(enemy);
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemyPickupDropper.DropFromEnemy(enemy);

            Unregister(enemy);
            //_enemySpawner.Despawn(enemy);
            
            enemy.PlayDeathAnimation(() =>
            {
                _enemySpawner.Despawn(enemy);
            });
        }
        
        private void OnGameplayAllowedChanged(bool isGameplayAllowed)
        {
            bool isPaused = !isGameplayAllowed;

            for (int i = 0; i < _enemies.Count; i++)
            {
                var enemy = _enemies[i];
                if (enemy == null)
                    continue;

                enemy.SetPaused(isPaused);
            }
        }
    }
}