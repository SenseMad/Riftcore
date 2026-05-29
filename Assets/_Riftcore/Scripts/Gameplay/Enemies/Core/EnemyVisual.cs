using System;
using DG.Tweening;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Core
{
    public sealed class EnemyVisual : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        
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
            //SetAlpha(1f);
            
            _scaleTween = transform.DOScale(Vector3.one, _spawnDuration).SetEase(Ease.OutBack);
        }

        public void PlayDeath(Action onComplete)
        {
            KillTween();
            
            _scaleTween = transform
                .DOScale(Vector3.zero, _deathDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
            
            /*var sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(Vector3.zero, _deathDuration).SetEase(Ease.InBack));
            
            foreach (Material material in _materials)
            {
                material.DOKill();

                sequence.Join(material.DOFade(0f, BaseColorId, _deathDuration));
            }

            sequence.OnComplete(() => onComplete?.Invoke());
            
            _scaleTween = sequence;*/
        }

        public void ResetVisual()
        {
            KillTween();
            
            transform.localScale = Vector3.one;
            //SetAlpha(1f);
        }

        private void SetAlpha(float alpha)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                Material material = _materials[i];

                if (!material.HasProperty(BaseColorId))
                    continue;

                Color color = material.GetColor(BaseColorId);
                color.a = alpha;
                material.SetColor(BaseColorId, color);
            }
        }

        private void KillTween()
        {
            _scaleTween?.Kill();
            _scaleTween = null;
            
            /*if (_materials == null)
                return;

            for (int i = 0; i < _materials.Length; i++)
                _materials[i]?.DOKill();*/
        }
    }
}