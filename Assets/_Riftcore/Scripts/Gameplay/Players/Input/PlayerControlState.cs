using UnityEngine;

namespace Riftcore.Gameplay.Players.Input
{
    public sealed class PlayerControlState : MonoBehaviour
    {
        public bool InputEnabled { get; private set; } = true;
        
        public void SetInputEnabled(bool enabled)
        {
            InputEnabled = enabled;
        }
    }
}