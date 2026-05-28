using Riftcore.Core.GameState;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Attacker
{
    public sealed class EnemyTouchDamage : MonoBehaviour
    {
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private float _lastDamageTime;

        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            if (Time.time - _lastDamageTime < _enemy.EnemyData.TouchDamageInterval)
                return;
            
            if (!other.TryGetComponent(out Player player))
                return;
            
            _lastDamageTime = Time.time;
            
            player.Damageable.TakeDamage(_enemy.EnemyData.TouchDamage, gameObject);
            //player.TakeDamage(_enemy.EnemyData.TouchDamage);
        }
    }
}