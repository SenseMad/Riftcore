using Riftcore.Core.Cursor;
using Riftcore.Core.Game;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Players.Data;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Level
{
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private PlayerData _selectedPlayerData;
        
        [Inject] private readonly GameManager _gameManager;
        [Inject] private readonly PlayerFactory _playerFactory;
        [Inject] private readonly GameContext _gameContext;

        private void Start()
        {
            _gameManager.CurrentSelectedPlayerData = _selectedPlayerData;
            
            var player = _playerFactory.Create(_gameManager.CurrentSelectedPlayerData);
            _gameContext.SetPlayer(player);
            
            CursorController.HideCursor();
        }
    }
}