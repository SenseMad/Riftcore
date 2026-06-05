using DG.Tweening;
using Riftcore.Gameplay.HealthSystem;
using UnityEngine;

namespace Riftcore.Gameplay
{
    public sealed class HitFlash : MonoBehaviour
    {
        private static readonly int _baseColorId = Shader.PropertyToID("_BaseColor");
        
        [Header("Flash")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField, Min(0)] private int _flashCount = 3;
        [SerializeField, Min(0)] private float _totalDuration = 0.2f;
        
        private Renderer[] _renderers;
        private Material[] _materials;
        private Color[] _baseColors;

        private Health _health;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _materials = new Material[_renderers.Length];
            _baseColors = new Color[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
            {
                _materials[i] = _renderers[i].material;
                _baseColors[i] = _materials[i].GetColor(_baseColorId);
            }
            
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnTakeDamage += OnHit;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= OnHit;
        }
        
        private void OnHit(float damage)
        {
            if (!_enabled)
                return;

            int cycles = _flashCount * 2;
            float halfDuration = _totalDuration / cycles;

            for (int i = 0; i < _materials.Length; i++)
            {
                Material material = _materials[i];
                Color baseColor = _baseColors[i];

                material.DOKill();
                material.color = baseColor;
                
                material.DOColor(_flashColor, _baseColorId, halfDuration).SetEase(Ease.Linear).SetLoops(cycles, LoopType.Yoyo).SetUpdate(true)
                    .OnComplete(() =>
                {
                    material.SetColor(_baseColorId, baseColor);
                });
            }
        }
    }
}