using System;
using System.Collections;
using Riftcore.Core.Game;
using Riftcore.Gameplay.Players.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Riftcore.Gameplay.Players.UI
{
    public sealed class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TMP_Text _healthText;
        
        [Inject] private readonly GameContext _gameContext;

        private Player _player;

        private float _maxHealthFillWidth;
        private float _displayHealth;
        private float _targetHealth;
        
        private Coroutine _animateHealth;

        private void Awake()
        {
            _gameContext.OnPlayerSpawned += OnPlayerSpawned;
            
            _maxHealthFillWidth = _healthBar.rectTransform.sizeDelta.x;
        }

        private void OnPlayerSpawned(Player player)
        {
            Bind(player);
        }

        private void OnEnable()
        {
            if (_gameContext.Player != null)
                Bind(_gameContext.Player);
        }

        private void OnDisable()
        {
            Unbind();

            if (_animateHealth != null)
            {
                StopCoroutine(_animateHealth);
                _animateHealth = null;
            }
        }

        private void OnDestroy()
        {
            _gameContext.OnPlayerSpawned -= OnPlayerSpawned;
        }

        private void Bind(Player player)
        {
            Unbind();
            
            _player = player;
            
            _displayHealth = _player.Health.CurrentHealth;
            _targetHealth = _displayHealth;
            
            UpdateHealthText(_displayHealth);
            UpdateView(_displayHealth);
            
            _player.Health.OnChangeHealth += OnChangeHealth;

            _animateHealth = StartCoroutine(AnimateHealth());
        }

        private void Unbind()
        {
            if (_player != null)
                _player.Health.OnChangeHealth -= OnChangeHealth;

            _player = null;
        }

        private void OnChangeHealth(float health)
        {
            _targetHealth = health;
        }

        private void UpdateView(float health)
        {
            float width = _maxHealthFillWidth * Mathf.Clamp01(health / _player.Health.MaxHealth);
            _healthBar.rectTransform.sizeDelta = new Vector2(width, _healthBar.rectTransform.sizeDelta.y);
        }

        private void UpdateHealthText(float health)
        {
            _healthText.text = $"{Mathf.RoundToInt(health)}/{Mathf.RoundToInt(_player.Health.MaxHealth)}";
        }

        private IEnumerator AnimateHealth()
        {
            const float speed = 75f;

            while (true)
            {
                if (!Mathf.Approximately(_displayHealth, _targetHealth))
                {
                    _displayHealth = Mathf.MoveTowards(_displayHealth, _targetHealth, speed * Time.deltaTime);

                    UpdateHealthText(_displayHealth);
                    UpdateView(_displayHealth);
                }
                
                yield return null;
            }
        }
    }
}