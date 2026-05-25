using System.Collections;
using Riftcore.Core.GameState;
using Riftcore.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Riftcore.Pooling.IPoolable;

namespace Riftcore.Gameplay.Effects.Core
{
    public sealed class HitEffect : PoolItem, IPoolable
    {
        [SerializeField] private ParticleSystem _particleSystem;

        [Inject] private readonly EffectPool _effectPool;
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private Coroutine _returnCoroutine;

        private void OnEnable()
        {
            _gameplayLockService.OnGameplayAllowedChanged += OnGameplayAllowedChanged;
        }

        private void OnDisable()
        {
            _gameplayLockService.OnGameplayAllowedChanged -= OnGameplayAllowedChanged;
        }

        public void Play(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            gameObject.SetActive(true);
            
            _particleSystem.Play();
            
            if (_returnCoroutine != null)
                StopCoroutine(_returnCoroutine);

            _returnCoroutine = StartCoroutine(ReturnAfterPlay());
        }
        
        public void OnGetFromPool()
        {
            gameObject.SetActive(false);
        }

        public void OnReturnToPool()
        {
            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }
            
            _particleSystem.Stop(true,  ParticleSystemStopBehavior.StopEmittingAndClear);
            
            gameObject.SetActive(false);
        }

        private IEnumerator ReturnAfterPlay()
        {
            yield return new WaitUntil(() => !_particleSystem.IsAlive(true));
            
            _effectPool.Return(this);
        }
        
        private void OnGameplayAllowedChanged(bool isAllowed)
        {
            if (isAllowed)
                _particleSystem.Play();
            else
                _particleSystem.Pause();
        }
    }
}