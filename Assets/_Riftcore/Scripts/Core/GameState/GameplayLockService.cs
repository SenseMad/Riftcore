using System;
using System.Collections.Generic;

namespace Riftcore.Core.GameState
{
    public sealed class GameplayLockService
    {
        private readonly HashSet<object> _locks = new();
        
        public bool IsGameplayAllowed => _locks.Count == 0;
        
        public event Action<bool> OnGameplayAllowedChanged;

        public void Lock(object owner)
        {
            if (owner == null)
                return;

            bool wasAllowed = IsGameplayAllowed;
            _locks.Add(owner);
            
            if (wasAllowed != IsGameplayAllowed)
                OnGameplayAllowedChanged?.Invoke(IsGameplayAllowed);
        }

        public void Unlock(object owner)
        {
            if (owner == null)
                return;
            
            bool wasAllowed = IsGameplayAllowed;
            _locks.Remove(owner);
            
            if (wasAllowed != IsGameplayAllowed)
                OnGameplayAllowedChanged?.Invoke(IsGameplayAllowed);
        }

        public bool HasLock(object owner)
        {
            return owner != null && _locks.Contains(owner);
        }
    }
}