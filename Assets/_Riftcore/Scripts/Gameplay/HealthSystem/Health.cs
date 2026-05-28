using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Riftcore.Gameplay.HealthSystem
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField, Min(1)] private float _maxHealth = 100f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _maxHealth;

        public bool IsImmortal { get; private set; }
        public bool IsDead { get; private set; }

        public event Action<float> OnChangeHealth;
        public event Action<float> OnHeal;
        public event Action<float> OnTakeDamage;
        public event Action OnDied;
        
        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (IsImmortal || IsDead)
                return;

            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            float healthBefore = CurrentHealth;

            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);

            float damageAmount = healthBefore - CurrentHealth;
            if (damageAmount > 0)
            {
                OnTakeDamage?.Invoke(damageAmount);
                OnChangeHealth?.Invoke(CurrentHealth);
            }

            if (CurrentHealth <= 0)
                Die();
        }

        public void Heal(float amount)
        {
            if (IsDead)
                return;

            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            float healthBefore = CurrentHealth;

            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, _maxHealth);

            float healAmount = CurrentHealth - healthBefore;
            if (healAmount > 0)
            {
                OnHeal?.Invoke(healAmount);
                OnChangeHealth?.Invoke(CurrentHealth);
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetImmortal(bool isImmortal)
        {
            IsImmortal = isImmortal;
        }
        
        public void ResetHealth()
        {
            IsDead = false;
            CurrentHealth = _maxHealth;
            OnChangeHealth?.Invoke(CurrentHealth);
        }

        public void Die()
        {
            if (IsDead)
                return;

            IsDead = true;
            OnDied?.Invoke();
        }
    }
}