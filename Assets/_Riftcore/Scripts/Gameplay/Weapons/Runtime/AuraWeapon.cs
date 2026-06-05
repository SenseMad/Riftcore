using System.Collections.Generic;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Runtime
{
    public sealed class AuraWeapon : Weapon<AuraWeaponData>
    {
        private readonly List<Enemy> _enemies = new();

        private GameObject _effectInstance;

        public AuraWeapon(AuraWeaponData weaponData, Player player, EnemyGrid enemyGrid) 
            : base(weaponData, player, enemyGrid)
        {
            CreateEffect();
        }

        protected override void Attack(Enemy target)
        {
            _enemies.Clear();
            
            _enemyGrid.GetEnemiesInRadius(_player.transform.position, WeaponData.AttackRange, _enemies);

            foreach (var enemy in _enemies)
            {
                if (enemy == null)
                    continue;
                
                if (enemy.Health.IsDead)
                    continue;
                
                int finalDamage = CriticalDamage(out bool isCritical);
                enemy.TakeDamage(finalDamage);
            }
        }

        private void CreateEffect()
        {
            if (_weaponData.EffectPrefab == null)
                return;

            _effectInstance = Object.Instantiate(_weaponData.EffectPrefab, _player.transform);
            _effectInstance.transform.localPosition = Vector3.zero;
        }
    }
}