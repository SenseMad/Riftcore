using System;
using Riftcore.Core.GameState;

namespace Riftcore.Core.Cursor
{
    public sealed class CursorStateHandler : IDisposable
    {
        private readonly GameplayLockService _gameplayLockService;

        public CursorStateHandler(GameplayLockService gameplayLockService)
        {
            _gameplayLockService = gameplayLockService;
            
            _gameplayLockService.OnGameplayAllowedChanged += OnGameplayAllowedChanged;
        }

        private void OnGameplayAllowedChanged(bool gameplayAllowed)
        {
            if (gameplayAllowed)
                CursorController.HideCursor();
            else
                CursorController.ShowCursor();
        }

        public void Dispose()
        {
            _gameplayLockService.OnGameplayAllowedChanged -= OnGameplayAllowedChanged;
        }
    }
}