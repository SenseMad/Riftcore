using Riftcore.Gameplay.Enemies.Core;
using Riftcore.Gameplay.Pickups.Data;
using Riftcore.Gameplay.Pickups.Runtime;
using UnityEngine;

namespace Riftcore.Gameplay.Enemies.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Riftcore/Enemy/Data/EnemyData")]
    public sealed class EnemyData : ScriptableObject
    {
        [field: SerializeField] public Enemy EnemyPrefab { get; private set; }
        
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float SpeedWalking { get; private set; }
        [field: SerializeField, Min(0)] public float Acceleration { get; private set; }
        /*[field: SerializeField, Min(0)] public float GroundCheckDistance { get; private set; }
        [field: SerializeField] public LayerMask GroundLayerMask { get; private set; }*/
        
        [field: Header("Attack")]
        [field: SerializeField, Min(0)] public float AttackRange { get; private set; }
        [field: SerializeField, Min(0)] public float AttackSpeed { get; private set; }
        [field: SerializeField, Min(0)] public float Damage { get; private set; }
        
        [field: Header("Health")]
        [field: SerializeField, Min(0)] public int MaxHealth { get; private set; }
        
        [field: Header("PickupDrop")]
        [field: SerializeField] public ExperiencePickup ExperiencePickupPrefab { get; private set; }
        [field: SerializeField, Min(0)] public int ExperienceAmount { get; private set; }
        [field: SerializeField] public PickupDropEntry[] PickupDropEntries { get; private set; }
    }
}