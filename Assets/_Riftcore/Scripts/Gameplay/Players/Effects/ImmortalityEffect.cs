using System.Collections;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Effects
{
    public sealed class ImmortalityEffect : MonoBehaviour
    {
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private Player _player;
        
        private Coroutine _immortalityRoutine;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnDestroy()
        {
            if (_immortalityRoutine != null)
            {
                StopCoroutine(_immortalityRoutine);
                _immortalityRoutine = null;
            }
            
            _player.Health.SetImmortal(false);
        }

        public void Activate(float duration)
        {
            if (_immortalityRoutine != null)
                StopCoroutine(_immortalityRoutine);
            
            _immortalityRoutine = StartCoroutine(ActivateImmortalityEffect(duration));
        }

        private IEnumerator ActivateImmortalityEffect(float duration)
        {
            _player.Health.SetImmortal(true);

            float timer = duration;
            while (timer > 0)
            {
                if (_gameplayLockService.IsGameplayAllowed)
                    timer -= Time.deltaTime;
                
                yield return null;
            }
            
            _player.Health.SetImmortal(false);
            _immortalityRoutine = null;
        }
    }
}