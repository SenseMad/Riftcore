using Riftcore.Gameplay.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Stats
{
    [CreateAssetMenu(fileName = "PlayerGameStatistics", menuName = "Riftcore/Player/PlayerGameStatistics")]
    public sealed class PlayerGameStatistics : ScriptableObject
    {
        [field: SerializeField] public GameStatistics BaseStatistics { get; private set; }
    }
}