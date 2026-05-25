using System;
using Riftcore.Core.GameState;
using Zenject;

namespace Riftcore.Core.Pause
{
    public sealed class PauseService
    {
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        public bool IsPaused { get; private set; }
        
        public event Action<bool> OnPauseChanged;

        public void Pause()
        {
            if (IsPaused)
                return;
            
            IsPaused = true;
            _gameplayLockService.Lock(this);
            
            OnPauseChanged?.Invoke(true);
        }

        public void Resume()
        {
            if (!IsPaused)
                return;
            
            IsPaused = false;
            _gameplayLockService.Unlock(this);
            
            OnPauseChanged?.Invoke(false);
        }

        public void Toggle()
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }
}