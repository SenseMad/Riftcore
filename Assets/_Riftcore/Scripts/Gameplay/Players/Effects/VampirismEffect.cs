using Riftcore.Gameplay.Players.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Effects
{
    public sealed class VampirismEffect : MonoBehaviour
    {
        private Player _player;

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
        }
        
        private void OnTakeDamage(float damage)
        {
            float vampirism = _player.GameStatistics.HealthStatistics.Vampirism;
            if (vampirism <= 0)
                return;
            
            float healAmount = damage * vampirism;
            _player.Health.Heal(healAmount);
        }
    }
}