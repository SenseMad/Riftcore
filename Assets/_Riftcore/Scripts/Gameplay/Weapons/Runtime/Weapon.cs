using System.Collections.Generic;
using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using Riftcore.Gameplay.Stats;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.World.Grid;
using UnityEngine;

namespace Riftcore.Gameplay.Weapons.Runtime
{
    public abstract class Weapon<TWeaponData> : Weapon where TWeaponData : WeaponData
    {
        protected readonly TWeaponData _weaponData;
        protected readonly Player _player;
        protected readonly EnemyGrid _enemyGrid;
        
        private float _attackCooldown;
        private float _attackTimer;

        public override WeaponData WeaponData => _weaponData;
        
        public CombatStatistics CombatStatistics { get; private set; }

        public Weapon(TWeaponData weaponData, Player player, EnemyGrid enemyGrid)
        {
            _weaponData = weaponData;
            _player = player;
            _enemyGrid = enemyGrid;

            CombatStatistics = new CombatStatistics(_weaponData.CombatStatistics);

            _attackCooldown = 60f / _weaponData.AttackPerMinutes;
        }
        
        public override void Tick()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer < _attackCooldown)
                return;

            _attackTimer = 0;
            
            var target = FindNearestEnemy(_weaponData.AttackRange);
            if (target == null)
                return;
            
            Attack(target);
        }

        public override void ApplyModifier(StatModifierData statModifierData)
        {
            CombatStatistics.ApplyModifier(statModifierData);
        }

        public override bool TryGetStatValue(StatType statType, out float value)
        {
            return CombatStatistics.TryGetValue(statType, out value);
        }

        protected abstract void Attack(Enemy target);

        protected int CriticalDamage(out bool isCritical)
        {
            float baseDamage = CombatStatistics.Damage * _player.GameStatistics.CombatStatistics.Damage;
            float critChance = CombatStatistics.CritChance + _player.GameStatistics.CombatStatistics.CritChance;
            float critDamage = CombatStatistics.CritDamage + _player.GameStatistics.CombatStatistics.CritDamage;
            
            isCritical = Random.value < critChance / 100f;
            
            float finalDamage = isCritical ? baseDamage + critDamage : baseDamage;

            return Mathf.CeilToInt(finalDamage);
        }

        protected Enemy FindNearestEnemy(float range)
        {
            var list = new List<Enemy>();
            _enemyGrid.GetEnemiesInRadius(_player.transform.position, range, list);
            
            Enemy nearest = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in list)
            {
                float sqrDistance = (_player.transform.position - enemy.transform.position).sqrMagnitude;
                if (sqrDistance < minDistance)
                {
                    minDistance = sqrDistance;
                    nearest = enemy;
                }
            }

            return nearest;
        }
    }
}