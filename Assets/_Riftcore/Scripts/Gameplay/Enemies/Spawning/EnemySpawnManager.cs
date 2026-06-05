using System.Collections.Generic;
using Riftcore.Core.Game;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Enemies.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Infrastructure.Logging;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Riftcore.Gameplay.Enemies.Spawning
{
    public sealed class EnemySpawnManager : MonoBehaviour
    {
        [Inject] private readonly GameContext _gameContext;
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        [Inject] private readonly EnemyManager _enemyManager;
        [Inject] private readonly EnemySpawner _enemySpawner;
        [Inject] private readonly EnemySpawnPointFinder _enemySpawnPointFinder;
        [Inject] private readonly EnemySpawnSettings _enemySpawnSettings;
        
        private const float DifficultyGrowTime = 600f;
        
        private readonly Dictionary<EnemySpawnEntry, float> _nextSpawnTime = new();

        private Player _player;
        private float _elapsedTime;
        private bool _playerReady;

        private void Awake()
        {
            _gameContext.OnPlayerSpawned += OnPlayerSpawned;

            if (_gameContext.Player != null)
                OnPlayerSpawned(_gameContext.Player);
        }

        private void OnDestroy()
        {
            _gameContext.OnPlayerSpawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(Player player)
        {
            _player = player;
            
            _enemySpawnPointFinder.SetPlayer(player);
            _playerReady = true;
        }

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            if (!_playerReady)
                return;
            
            _elapsedTime += Time.deltaTime;
            
            foreach (var enemySpawnEntry in _enemySpawnSettings.EnemySpawnEntries)
                TrySpawn(enemySpawnEntry);
        }
        
        private void TrySpawn(EnemySpawnEntry enemySpawnEntry)
        {
            if (enemySpawnEntry == null)
                return;
            
            float time = _elapsedTime;
            
            if (time < enemySpawnEntry.MinAppearTime)
                return;

            float normalizedTime = Mathf.Clamp01(time / DifficultyGrowTime);
            
            float difficultyMultiplier = _enemySpawnSettings.UseDifficultyForInterval ? GetDifficultyMultiplier() : 1f;
            float intervalMultiplier = enemySpawnEntry.IntervalMultiplierCurve.Evaluate(normalizedTime);
            float interval = enemySpawnEntry.BaseSpawnInterval * intervalMultiplier / difficultyMultiplier;
            interval = Mathf.Max(_enemySpawnSettings.MinSpawnInterval, interval);
            
            float nextTime = _nextSpawnTime.GetValueOrDefault(enemySpawnEntry, 0f);
            
            if (time < nextTime)
                return;
            
            if (Random.value > enemySpawnEntry.BaseSpawnChance)
                return;

            int groupSize = Mathf.Clamp(Mathf.RoundToInt(enemySpawnEntry.GroupSizeCurve.Evaluate(normalizedTime)), 1, enemySpawnEntry.MaxGroupSize);
            
            float groupSpawnChance = enemySpawnEntry.GroupSpawnChanceCurve.Evaluate(normalizedTime);

            bool isSpawnGroup = Random.value <= groupSpawnChance;
            bool spawned = false;
            if (!isSpawnGroup || groupSize <= 1)
            {
                var enemy = SpawnEnemyNearPlayer(enemySpawnEntry.EnemyData.EnemyPrefab);
                if (enemy != null)
                {
                    enemy.Initialize(enemySpawnEntry.EnemyData);
                    _enemyManager.Register(enemy);
                    spawned = true;
                }
            }
            else
            {
                var enemies = SpawnGroup(enemySpawnEntry, groupSize);
                if (enemies != null && enemies.Count > 0)
                {
                    foreach (var enemy in enemies)
                    {
                        if (enemy == null)
                            continue;
                    
                        enemy.Initialize(enemySpawnEntry.EnemyData);
                        _enemyManager.Register(enemy);
                    }

                    spawned = true;
                }
            }
            
            if (spawned)
                _nextSpawnTime[enemySpawnEntry] = time + interval;
        }

        private float GetDifficultyMultiplier()
        {
            if (_player == null)
                return 1f;

            return 1f + _player.GameStatistics.LevelStatistics.Difficulty / 100f;
        }
        
        private Enemy SpawnEnemyNearPlayer(Enemy enemyPrefab)
        {
            if (_enemySpawnPointFinder.TryFindSpawnPoint(out var spawnPoint))
                return SpawnEnemyAt(enemyPrefab, spawnPoint);
            
            GameLog.Warning("Не удалось найти подходящее место для спавна врага");
            return null;
        }

        private Enemy SpawnEnemyAt(Enemy enemyPrefab, Vector3 position)
        {
            Enemy enemy = _enemySpawner.SpawnEnemy(enemyPrefab, position);

            if (enemy.TryGetComponent(out Collider collider))
            {
                float offsetY = collider.bounds.extents.y;
                enemy.transform.position = position + Vector3.up * offsetY;
            }
            
            enemy.ResetSpawnPosition();

            return enemy;
        }

        public List<Enemy> SpawnGroup(EnemySpawnEntry enemySpawnEntry, int count)
        {
            if (count <= 0)
                return null;
            
            List<Enemy> spawnedEnemies = new();
            
            float difficultyMultiplier = GetDifficultyMultiplier();
            int finalCount = Mathf.Clamp(Mathf.RoundToInt(count * difficultyMultiplier), 1, enemySpawnEntry.MaxGroupSize);
            if (finalCount == 1)
            {
                Enemy singleEnemy = SpawnEnemyNearPlayer(enemySpawnEntry.EnemyData.EnemyPrefab);
                if (singleEnemy != null)
                    spawnedEnemies.Add(singleEnemy);
                
                return spawnedEnemies;
            }

            if (!_enemySpawnPointFinder.TryFindSpawnPoint(out var groupCenter))
            {
                GameLog.Warning("Не удалось найти подходящее место для спавна пачки врагов");
                return spawnedEnemies;
            }

            float groupRadius = Mathf.Clamp(finalCount * enemySpawnEntry.GroupRadiusPerEnemy, 
                enemySpawnEntry.MinGroupRadius, enemySpawnEntry.MaxGroupRadius);

            for (int i = 0; i < finalCount; i++)
            {
                Vector3 offset = new(Random.Range(-groupRadius, groupRadius), 0, Random.Range(-groupRadius, groupRadius));
                Vector3 spawnPosition = groupCenter + offset;

                if (_enemySpawnPointFinder.FindGroundBelow(spawnPosition, out var finalPosition)
                    && _enemySpawnPointFinder.IsPositionFree(finalPosition))
                {
                    var enemy = SpawnEnemyAt(enemySpawnEntry.EnemyData.EnemyPrefab, finalPosition);
                    spawnedEnemies.Add(enemy);
                }
            }
            
            return spawnedEnemies;
        }
    }
}