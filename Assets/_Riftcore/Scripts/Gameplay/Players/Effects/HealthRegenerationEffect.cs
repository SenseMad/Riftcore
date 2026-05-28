using System.Collections;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Effects
{
    public sealed class HealthRegenerationEffect : MonoBehaviour
    {
        [Tooltip("Через сколько секунд восстанавливать здоровье после получения урона")]
        [SerializeField, Min(0)] private float _regenerationDelay = 3.0f;
        
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private Player _player;
        
        private Coroutine _regenerationCoroutine;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnEnable()
        {
            _player.Health.OnTakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            _player.Health.OnTakeDamage -= OnTakeDamage;

            StopRegeneration();
        }

        private void OnTakeDamage(float damage)
        {
            if (_player.GameStatistics.HealthStatistics.RegenerationHealth <= 0)
                return;
            
            if (_player.Health.CurrentHealth >= _player.Health.MaxHealth)
                return;

            StopRegeneration();

            _regenerationCoroutine = StartCoroutine(Regeneration());
        }

        private void StopRegeneration()
        {
            if (_regenerationCoroutine == null)
                return;
            
            StopCoroutine(_regenerationCoroutine);
            _regenerationCoroutine = null;
        }

        private IEnumerator Regeneration()
        {
            float delayTimer = _regenerationDelay;
            while (delayTimer > 0)
            {
                if (_gameplayLockService.IsGameplayAllowed)
                    delayTimer -= Time.deltaTime;
                
                yield return null;
            }
            
            while (!_player.Health.IsDead && _player.Health.CurrentHealth < _player.Health.MaxHealth)
            {
                if (!_gameplayLockService.IsGameplayAllowed)
                {
                    yield return null;
                    continue;
                }
                
                float regeneration = _player.GameStatistics.HealthStatistics.RegenerationHealth;
                if (regeneration <= 0)
                    break;
                
                float healAmount = regeneration * Time.deltaTime;
                
                _player.Health.Heal(healAmount);
                
                yield return null;
            }

            _regenerationCoroutine = null;
        }
    }
}