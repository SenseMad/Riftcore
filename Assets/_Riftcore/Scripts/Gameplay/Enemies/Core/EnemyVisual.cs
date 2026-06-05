using System;
using DG.Tweening;
using Riftcore.Core.GameState;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private float _spawnDuration = 0.25f;
        [SerializeField] private float _deathDuration = 0.35f;

        private Tween _scaleTween;
        private Renderer[] _renderers;
        private Material[] _materials;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _materials = new Material[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
                _materials[i] = _renderers[i].material;
        }

        private void OnDestroy()
        {
            KillTween();
        }

        public void PlaySpawn()
        {
            KillTween();

            transform.localScale = Vector3.zero;
            
            _scaleTween = transform.DOScale(Vector3.one, _spawnDuration).SetEase(Ease.OutBack);
        }

        public void PlayDeath(Action onComplete)
        {
            KillTween();
            
            _scaleTween = transform
                .DOScale(Vector3.zero, _deathDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void ResetVisual()
        {
            KillTween();
            
            transform.localScale = Vector3.one;
        }

        private void KillTween()
        {
            _scaleTween?.Kill();
            _scaleTween = null;
        }
    }
}