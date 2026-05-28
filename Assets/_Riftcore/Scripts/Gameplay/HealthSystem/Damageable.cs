using UnityEngine;

namespace Riftcore.Gameplay.HealthSystem
{
    [RequireComponent(typeof(Health))]
    public sealed class Damageable : MonoBehaviour, IDamageable
    {
        private Health _health;
        
        public GameObject LastDamageAttacker { get; private set; }

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        public void TakeDamage(float damage)
        {
            _health.TakeDamage(damage);
        }
        
        public void TakeDamage(float damage, GameObject attacker)
        {
            LastDamageAttacker = attacker;

            _health.TakeDamage(damage);
        }

        public void SetDamageAttacker(GameObject attacker)
        {
            LastDamageAttacker = attacker;
        }
    }
}