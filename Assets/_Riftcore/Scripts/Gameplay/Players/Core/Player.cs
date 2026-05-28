using Riftcore.Gameplay.HealthSystem;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Players.Combat;
using Riftcore.Gameplay.Players.Data;
using Riftcore.Gameplay.Players.Effects;
using Riftcore.Gameplay.Players.Input;
using Riftcore.Gameplay.Players.Movement;
using Riftcore.Gameplay.Players.PlayerCamera;
using Riftcore.Gameplay.Projectiles.HitHandling.Interfaces;
using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Core
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Player : MonoBehaviour, IProjectileTarget
    {
        public PlayerData PlayerData  { get; private set; }
        public GameStatistics GameStatistics { get; private set; }
        
        public PlayerControlState PlayerControlState { get; private set; }
        public PlayerCameraController PlayerCameraController { get; private set; }
        public PlayerMovementController PlayerMovementController { get; private set; }
        public PlayerWeaponController PlayerWeaponController { get; private set; }
        public PlayerEffects PlayerEffects { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }
        
        public CharacterController CharacterController { get; private set; }
        
        public Health Health { get; private set; }
        public Damageable Damageable { get; private set; }
        
        public Collider Collider { get; private set; }
        
        public Transform Transform => transform;

        private void Awake()
        {
            PlayerControlState = GetComponentInChildren<PlayerControlState>();
            PlayerCameraController = GetComponentInChildren<PlayerCameraController>();
            PlayerMovementController = GetComponentInChildren<PlayerMovementController>();
            PlayerWeaponController = GetComponentInChildren<PlayerWeaponController>();
            PlayerEffects = GetComponentInChildren<PlayerEffects>();
            
            PlayerInventory = new PlayerInventory();

            CharacterController = GetComponent<CharacterController>();
            
            Health = GetComponent<Health>();
            Damageable = GetComponent<Damageable>();
            
            Collider = GetComponent<Collider>();
        }

        public void Bind(PlayerData playerData, GameStatistics gameStatistics)
        {
            PlayerData = playerData;
            GameStatistics = gameStatistics;
        }

        public void SetActiveControl(bool value)
        {
            if (PlayerControlState != null)
                PlayerControlState.SetInputEnabled(value);

            CharacterController.enabled = value;
        }

        public void TakeDamage(float damage)
        {
            Health.TakeDamage(damage);
        }
        
        /*public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            SetActiveControl(false);

            Vector3 oldPosition = transform.position;

            if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f))
                position = hit.point;

            transform.SetPositionAndRotation(position, rotation);

            Physics.SyncTransforms();

            Vector3 delta = position - oldPosition;
            PlayerCameraController.SetYawFromRotation(rotation);
            PlayerCameraController.ForceCameraWarp(delta);

            SetActiveControl(true);
        }*/
    }
}