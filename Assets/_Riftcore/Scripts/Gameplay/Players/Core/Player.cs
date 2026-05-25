using Riftcore.Gameplay.Inventory;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Players.Combat;
using Riftcore.Gameplay.Players.Data;
using Riftcore.Gameplay.Players.Input;
using Riftcore.Gameplay.Players.Movement;
using Riftcore.Gameplay.Players.PlayerCamera;
using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Core
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Player : MonoBehaviour
    {
        public PlayerData PlayerData  { get; private set; }
        public GameStatistics GameStatistics { get; private set; }
        
        public PlayerControlState PlayerControlState { get; private set; }
        public PlayerCameraController PlayerCameraController { get; private set; }
        public PlayerMovementController PlayerMovementController { get; private set; }
        public PlayerWeaponController PlayerWeaponController { get; private set; }
        public CharacterController CharacterController { get; private set; }
        
        public PlayerInventory PlayerInventory { get; private set; }
        
        public Collider Collider { get; private set; }

        private void Awake()
        {
            PlayerControlState = GetComponentInChildren<PlayerControlState>();
            PlayerCameraController = GetComponentInChildren<PlayerCameraController>();
            PlayerMovementController = GetComponentInChildren<PlayerMovementController>();
            PlayerWeaponController = GetComponentInChildren<PlayerWeaponController>();

            CharacterController = GetComponent<CharacterController>();
            
            PlayerInventory = new PlayerInventory();
            
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
        
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
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
        }
    }
}