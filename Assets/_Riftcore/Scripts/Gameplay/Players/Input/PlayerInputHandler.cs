using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        private GameInputActions _inputActions;
        private IPlayerInputProvider _playerInputProvider;

        private void Awake()
        {
            _inputActions = new GameInputActions();
            _playerInputProvider = new InputActionReader(_inputActions);
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
        }

        public PlayerInputData GetInput()
        {
            if (_playerInputProvider == null)
                return default;
            
            return _playerInputProvider.GetInput();
        }
    }
}