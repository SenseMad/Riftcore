using System.Collections.Generic;
using Riftcore.Gameplay.Inventory.Data;
using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Players.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Riftcore/Player/Data/PlayerData")]
    public sealed class PlayerData : ScriptableObject
    {
        [field: Header("UI")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: Header("Prefab")]
        [field: SerializeField] public Player PlayerPrefab { get; private set; }
        
        [field: Header("Start Items")]
        [field: SerializeField] public List<ItemData> StartItems { get; private set; }
        
        [field: Header("Stats")]
        [field: SerializeField] public PlayerGameStatistics PlayerGameStatistics { get; private set; }
    }
}