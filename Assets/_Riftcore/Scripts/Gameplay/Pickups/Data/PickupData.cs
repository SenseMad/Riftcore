using Riftcore.Gameplay.Effects.Core;
using UnityEngine;

namespace Riftcore.Gameplay.Pickups.Data
{
    [CreateAssetMenu(fileName = "PickupData", menuName = "Riftcore/Pickup/Data/PickupData")]
    public sealed class PickupData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float AttractSpeed { get; private set; }
        
        [field: Header("Visual")]
        [field: SerializeField] public Effect PickupEffectPrefab { get; private set; }
    }
}