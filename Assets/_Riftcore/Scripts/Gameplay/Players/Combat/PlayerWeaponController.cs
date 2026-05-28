using System.Collections.Generic;
using System.Linq;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Inventory.Core;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Weapons.Data;
using Riftcore.Gameplay.Weapons.Factories;
using Riftcore.Gameplay.Weapons.Runtime;
using Riftcore.Infrastructure.Logging;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Players.Combat
{
    public sealed class PlayerWeaponController : MonoBehaviour, IPlayerTickable
    {
        [Inject] private readonly WeaponFactory _weaponFactory;
        [Inject] private readonly WeaponDatabase _weaponDatabase;
        [Inject] private readonly GameplayLockService _gameplayLockService;

        private Player _player;
        
        private readonly List<Weapon> _weapons = new();

        private PlayerTickRunner _playerTickRunner;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            
            _playerTickRunner = GetComponentInParent<PlayerTickRunner>();
            if (_playerTickRunner == null)
            {
                GameLog.Error($"{nameof(PlayerWeaponController)}: PlayerTickRunner not found.");
                return;
            }
        }

        private void Start()
        {
            _player.PlayerInventory.OnItemAdded += OnItemAdded;

            foreach (var item in _player.PlayerInventory.Items)
                OnItemAdded(item);
        }

        private void OnEnable()
        {
            _playerTickRunner?.Register(this);
        }

        private void OnDisable()
        {
            _playerTickRunner?.Unregister(this);
        }
        
        private void OnDestroy()
        {
            _player.PlayerInventory.OnItemAdded -= OnItemAdded;
        }

        public void Tick()
        {
            if (!_gameplayLockService.IsGameplayAllowed)
                return;
            
            for (int i = 0; i < _weapons.Count; i++)
            {
                _weapons[i].Tick();
            }
        }

        public void AddWeapon(ItemData itemData)
        {
            if (itemData == null)
                return;
            
            if (_weapons.Any(x => x.WeaponData.ItemData.Id == itemData.Id))
                return;
            
            WeaponData weaponData = _weaponDatabase.GetWeaponData(itemData);
            if (weaponData == null)
            {
                GameLog.Error($"WeaponData not found for ItemData: {itemData.Name}");
                return;
            }
            
            Weapon weapon = _weaponFactory.Create(weaponData, _player);
            if (weapon == null)
                return;
                
            _weapons.Add(weapon);
        }

        public Weapon GetWeapon(ItemData itemData)
        {
            return _weapons.FirstOrDefault(x => x.WeaponData.ItemData.Id == itemData.Id);
        }
        
        private void OnItemAdded(InventoryItem inventoryItem)
        {
            if (inventoryItem.ItemData.ItemCategory != ItemCategory.Weapon)
                return;
            
            AddWeapon(inventoryItem.ItemData);
        }
    }
}