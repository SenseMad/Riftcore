using Riftcore.Core.GameState;
using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Weapons.Runtime;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Orbits
{
    public sealed class OrbitObject : MonoBehaviour
    {
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private Player _player;
        private OrbitWeapon _orbitWeapon;

        private float _angle;
        private float _radius;
        private float _rotationSpeed;
        
        private Vector3 _offsetPosition;

        public void Tick()
        {
            _angle += _rotationSpeed * Time.deltaTime;
            
            float radians = _angle * Mathf.Deg2Rad;
            Vector3 offsetRadians = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * _radius;
            
            transform.position = _player.transform.position + _offsetPosition + offsetRadians;
        }

        public void Initialize(Player player, OrbitWeapon orbitWeapon, Vector3 offsetPosition, float angle, float radius, float rotationSpeed)
        {
            _player = player;
            _orbitWeapon = orbitWeapon;
            _offsetPosition = offsetPosition;
            _angle = angle;
            _radius = radius;
            _rotationSpeed = rotationSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            if (!other.TryGetComponent(out Enemy enemy))
                return;
            
            _orbitWeapon.DamageEnemy(enemy);
        }
    }
}