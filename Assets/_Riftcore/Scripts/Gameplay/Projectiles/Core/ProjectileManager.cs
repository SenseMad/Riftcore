using System.Collections.Generic;
using Riftcore.Core.GameState;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class ProjectileManager : MonoBehaviour
    {
        [Inject] private readonly ProjectilePool _projectilePool;
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private readonly List<Projectile> _activeProjectiles = new();

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < _activeProjectiles.Count; i++)
            {
                var projectile = _activeProjectiles[i];
                if (projectile == null)
                    continue;
                
                projectile.Tick(deltaTime);
            }
        }

        public void Register(Projectile projectile)
        {
            _activeProjectiles.Add(projectile);
        }

        public void Unregister(Projectile projectile)
        {
            _activeProjectiles.Remove(projectile);
            
            _projectilePool.Return(projectile);
        }
    }
}