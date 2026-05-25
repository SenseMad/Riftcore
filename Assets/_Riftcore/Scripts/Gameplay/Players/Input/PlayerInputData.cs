using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public struct PlayerInputData
    {
        public Vector2 Move;
        public Vector2 Look;

        public bool JumpPressed;
        public bool SprintHeld;
    }
}