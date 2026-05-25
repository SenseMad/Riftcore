using System.Collections.Generic;
using Riftcore.Core.GameState;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Core
{
    public sealed class PlayerTickRunner : MonoBehaviour
    {
        [Inject] private readonly GameplayLockService _gameplayLockService;
        
        private readonly List<IPlayerTickable> _playerTickables = new();
        private readonly List<IPlayerLateTickable> _playerLateTickables = new();

        private void Update()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            for (int i = 0; i < _playerTickables.Count; i++)
            {
                if (_playerTickables[i] == null)
                    continue;
                
                _playerTickables[i].Tick();
            }
        }

        private void LateUpdate()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            for (int i = 0; i < _playerLateTickables.Count; i++)
            {
                if (_playerLateTickables[i] == null)
                    continue;
                
                _playerLateTickables[i].LateTick();
            }
        }

        public void Register(IPlayerTickable playerTickable)
        {
            if (playerTickable == null || _playerTickables.Contains(playerTickable))
                return;
            
            _playerTickables.Add(playerTickable);
        }

        public void Unregister(IPlayerTickable playerTickable)
        {
            if (playerTickable == null)
                return;
            
            _playerTickables.Remove(playerTickable);
        }
        
        public void Register(IPlayerLateTickable playerLateTickable)
        {
            if (playerLateTickable == null || _playerLateTickables.Contains(playerLateTickable))
                return;
            
            _playerLateTickables.Add(playerLateTickable);
        }

        public void Unregister(IPlayerLateTickable playerLateTickable)
        {
            if (playerLateTickable == null)
                return;
            
            _playerLateTickables.Remove(playerLateTickable);
        }
    }
}