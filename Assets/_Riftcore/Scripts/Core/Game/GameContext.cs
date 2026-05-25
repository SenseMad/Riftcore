using System;
using Riftcore.Gameplay.Players;
using Riftcore.Gameplay.Players.Core;

namespace Riftcore.Core.Game
{
    public sealed class GameContext
    {
        public Player Player { get; private set; }
        
        public event Action<Player> OnPlayerSpawned;
        
        public void SetPlayer(Player player)
        {
            Player = player;
            
            OnPlayerSpawned?.Invoke(player);
        }
    }
}