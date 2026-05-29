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
        [field: SerializeField, Min(0f)] public float RotationSpeed { get; private set; } = 720f;
        /*[field: SerializeField, Min(0)] public float GroundCheckDistance { get; private set; }
        [field: SerializeField] public LayerMask GroundLayerMask { get; private set; }*/
        
        [field: Header("Attack")]
        [field: SerializeField, Min(0)] public float AttackRange { get; private set; }
        [field: SerializeField, Min(0)] public float AttackSpeed { get; private set; }
        [field: SerializeField, Min(0)] public float Damage { get; private set; }
        [field: SerializeField, Min(0)] public float TouchDamage { get; private set; }
        [field: SerializeField, Min(0)] public float TouchDamageInterval { get; private set; }
        
        [field: Header("Climbing")]
        [field: SerializeField, Min(0)] public float ClimbSpeed { get; private set; } = 4f;
        [field: SerializeField, Min(0)] public float ClimbHorizontalMultiplier { get; private set; } = 0.2f;
        [field: SerializeField, Min(0)] public float WallCheckDistance { get; private set; } = 0.8f;
        [field: SerializeField, Min(0)] public float WallCheckRadius { get; private set; } = 0.35f;
        
        [field: Header("Health")]
        [field: SerializeField, Min(0)] public int MaxHealth { get; private set; }
        
        [field: Header("PickupDrop")]
        [field: SerializeField] public ExperiencePickup ExperiencePickupPrefab { get; private set; }
        [field: SerializeField, Min(0)] public int ExperienceAmount { get; private set; }
        [field: SerializeField] public PickupDropEntry[] PickupDropEntries { get; private set; }
    }
}