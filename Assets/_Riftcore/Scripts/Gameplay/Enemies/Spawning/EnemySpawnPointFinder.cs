using Riftcore.Gameplay.Enemies.Installers;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Spawning
{
    public sealed class EnemySpawnPointFinder
    {
        //private readonly GameContext _gameContext;
        private Player _player;
        
        private readonly EnemyGlobalSpawnInstallerData _enemyGlobalSpawnInstallerData;
        
        //private Player Player => _gameContext.Player;

        public EnemySpawnPointFinder(EnemyGlobalSpawnInstallerData enemyGlobalSpawnInstallerData)
        {
            _enemyGlobalSpawnInstallerData = enemyGlobalSpawnInstallerData;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }
        
        public bool TryFindSpawnPoint(out Vector3 spawnPoint)
        {
            const int maxAttempts = 20;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomPoint = GetSpawnPointInFrontOfPlayer();
                
                if (!FindGroundBelow(randomPoint, out Vector3 groundPoint))
                    continue;
                
                if (!IsPositionFree(groundPoint))
                    continue;
                
                spawnPoint = groundPoint;
                return true;
            }
            
            spawnPoint = Vector3.zero;
            return false;
        }
        
        /// <summary>
        /// Поиск поверхности
        /// </summary>
        public bool FindGroundBelow(Vector3 startPoint, out Vector3 groundPosition)
        {
            const float probeRadius = 0.5f;
            const int raysCount = 8;
            
            float highestY = float.MinValue;
            Vector3 bestPoint = Vector3.zero;

            for (int i = 0; i < raysCount; i++)
            {
                float angle = i * Mathf.PI * 2f / raysCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * probeRadius;
                Vector3 rayOrigin = startPoint + offset + Vector3.up * _enemyGlobalSpawnInstallerData.SpawnHeight;
                
                Ray ray = new Ray(rayOrigin, Vector3.down);
                bool hasHit = Physics.Raycast(ray, out RaycastHit hit, _enemyGlobalSpawnInstallerData.SpawnHeight * 2,
                    _enemyGlobalSpawnInstallerData.GroundMask);
                if (!hasHit || hit.point.y <= highestY)
                    continue;
                
                highestY = hit.point.y;
                bestPoint = hit.point;
            }

            if (Mathf.Approximately(highestY, float.MinValue))
            {
                groundPosition = Vector3.zero;
                return false;
            }

            bool blockedAbove = Physics.CheckSphere(bestPoint + Vector3.up, 0.5f,
                _enemyGlobalSpawnInstallerData.GroundMask);
            if (blockedAbove)
            {
                groundPosition = Vector3.zero;
                return false;
            }
            
            groundPosition = bestPoint;
            return true;
        }
        
        /// <summary>
        /// Генерация точки перед игроком
        /// </summary>
        private Vector3 GetSpawnPointInFrontOfPlayer()
        {
            Vector3 forward = _player.transform.forward.normalized;

            float halfAngle = _enemyGlobalSpawnInstallerData.SpawnAngle * 0.5f;
            float angle = Random.Range(-halfAngle, halfAngle);
            float distance = Random.Range(_enemyGlobalSpawnInstallerData.MinSpawnDistance, _enemyGlobalSpawnInstallerData.MaxSpawnDistance);

            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            
            return _player.transform.position + direction * distance;
        }
        
        /// <summary>
        /// Генерация случайной точки вокруг игрока
        /// </summary>
        private Vector3 GetRandomPointAroundPlayer()
        {
            float angle = Random.Range(0, 360f);
            float distance = Random.Range(_enemyGlobalSpawnInstallerData.MinSpawnDistance, _enemyGlobalSpawnInstallerData.MaxSpawnDistance);

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
            return _player.transform.position + offset;
        }
        
        /// <summary>
        /// Проверка пересечений
        /// </summary>
        public bool IsPositionFree(Vector3 position)
        {
            return !Physics.CheckSphere(position, _enemyGlobalSpawnInstallerData.CheckRadius, ~_enemyGlobalSpawnInstallerData.GroundMask);
        }
    }
}