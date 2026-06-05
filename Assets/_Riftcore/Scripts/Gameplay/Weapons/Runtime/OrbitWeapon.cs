using System.Collections.Generic;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Orbits;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Skills.Stats;
using Riftcore.Gameplay.Stats;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.World.Grid;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Weapons.Runtime
{
    public sealed class OrbitWeapon : Weapon<OrbitWeaponData>
    {
        private readonly DiContainer _container;
        
        private readonly OrbitStatistics _orbitStatistics;
        
        private readonly List<OrbitObject> _orbitObjects = new();
        
        public OrbitWeapon(OrbitWeaponData weaponData, Player player, EnemyGrid enemyGrid, DiContainer container) : base(weaponData, player, enemyGrid)
        {
            _container = container;
            _orbitStatistics = new OrbitStatistics(_weaponData.OrbitStatistics);

            SyncOrbitObjects();
        }

        public override void Tick()
        {
            base.Tick();

            for (int i = 0; i < _orbitObjects.Count; i++)
            {
                _orbitObjects[i].Tick();
            }
        }

        protected override void Attack(Enemy target) { }

        private void SyncOrbitObjects()
        {
            int targetCount = Mathf.FloorToInt(_orbitStatistics.OrbitObjectCount);

            while (_orbitObjects.Count < targetCount)
                AddOrbitObject();
            
            RecalculateAngles();
        }

        private void AddOrbitObject()
        {
            OrbitObject orbitObject =
                _container.InstantiatePrefabForComponent<OrbitObject>(_weaponData.OrbitObjectPrefab, _player.transform);
            
            orbitObject.transform.position += _weaponData.Offset;
            _orbitObjects.Add(orbitObject);

            RecalculateAngles();
        }
        
        public override void ApplyModifier(StatModifierData statModifierData)
        {
            base.ApplyModifier(statModifierData);
            
            _orbitStatistics.ApplyModifier(statModifierData);

            SyncOrbitObjects();
        }

        public override bool TryGetStatValue(StatType statType, out float value)
        {
            return base.TryGetStatValue(statType, out value) || _orbitStatistics.TryGetValue(statType, out value);
        }

        private void RecalculateAngles()
        {
            for (int i = 0; i < _orbitObjects.Count; i++)
            {
                float angle = 360f / _orbitObjects.Count * i;
                
                _orbitObjects[i].Initialize(_player, this, _weaponData.Offset, angle, 
                    _orbitStatistics.OrbitRadius, _orbitStatistics.OrbitRotationSpeed);
            }
        }

        public void DamageEnemy(Enemy enemy)
        {
            int finalDamage = CriticalDamage(out bool isCritical);
            enemy.TakeDamage(finalDamage);
        }
    }
}