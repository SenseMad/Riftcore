using System;
using Riftcore.Gameplay.Pickups.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Pickups.Data
{
    [Serializable]
    public sealed class PickupDropEntry
    {
        [field: SerializeField] public BasePickup PickupPrefab { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float Chance { get; private set; }
    }
}