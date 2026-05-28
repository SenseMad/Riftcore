using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Effects
{
    public sealed class SpikesEffect : MonoBehaviour
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
            float spikesDamage = _player.GameStatistics.HealthStatistics.Spikes;
            if (spikesDamage <= 0)
                return;

            var attacker = _player.Damageable.LastDamageAttacker;
            _player.Damageable.SetDamageAttacker(null);
            if (attacker == null)
                return;
            
            if (!attacker.TryGetComponent(out Enemy enemy))
                return;
            
            enemy.Health.TakeDamage(spikesDamage);
        }
    }
}