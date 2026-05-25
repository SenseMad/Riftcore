using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Players.Movement;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public sealed class PlayerInputRouter : MonoBehaviour, IPlayerTickable
    {
        [SerializeField] private PlayerTickRunner _playerTickRunner;
        [SerializeField] private PlayerControlState _playerControlState;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private PlayerMovementController  _playerMovementController; 
        [SerializeField] private PlayerStateController _playerStateController;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_playerTickRunner == null)
            {
                _playerTickRunner = GetComponentInParent<PlayerTickRunner>();
                if (_playerTickRunner == null)
                    return;
            }

            if (_playerControlState == null)
                _playerControlState = _playerTickRunner.GetComponentInChildren<PlayerControlState>();

            if (_playerInputHandler == null)
                _playerInputHandler = _playerTickRunner.GetComponentInChildren<PlayerInputHandler>();
            
            if (_playerMovementController == null)
                _playerMovementController = _playerTickRunner.GetComponentInChildren<PlayerMovementController>();
            
            if (_playerStateController == null)
                _playerStateController = _playerTickRunner.GetComponentInChildren<PlayerStateController>();
        }
#endif

        private void OnEnable()
        {
            _playerTickRunner?.Register(this);
        }

        private void OnDisable()
        {
            _playerTickRunner?.Unregister(this);
        }

        public void Tick()
        {
            if (_playerInputHandler == null)
                return;

            PlayerInputData inputData = default;

            if (_playerControlState == null || _playerControlState.InputEnabled)
                inputData = _playerInputHandler.GetInput();
            
            _playerStateController.Tick(inputData);
            _playerMovementController.Tick(inputData);
        }
    }
}