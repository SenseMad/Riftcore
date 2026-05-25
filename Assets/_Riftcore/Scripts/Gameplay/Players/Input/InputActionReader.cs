using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public sealed class InputActionReader : IPlayerInputProvider
    {
        private readonly GameInputActions _gameInputActions;
        
        public InputActionReader(GameInputActions gameGameInputActions)
        {
            _gameInputActions = gameGameInputActions;
        }

        public PlayerInputData GetInput()
        {
            return new PlayerInputData
            {
                Move = _gameInputActions.Player.Move.ReadValue<Vector2>(),
                Look = _gameInputActions.Player.Look.ReadValue<Vector2>(),
                JumpPressed =  _gameInputActions.Player.Jump.WasPressedThisFrame(),
                SprintHeld = _gameInputActions.Player.Sprint.IsPressed()
            };
        }
    }
}