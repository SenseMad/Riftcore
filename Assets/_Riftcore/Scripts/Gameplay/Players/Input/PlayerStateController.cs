using Riftcore.Gameplay.Players.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public sealed class PlayerStateController : MonoBehaviour
    {
        public PlayerState CurrentState { get; private set; }

        public void Tick(PlayerInputData inputData)
        {
            PlayerState state = CurrentState;

            state.CanSprint = true;
            state.IsSprinting = inputData.SprintHeld && CurrentState.CanSprint;
            
            CurrentState = state;
        }
    }
}