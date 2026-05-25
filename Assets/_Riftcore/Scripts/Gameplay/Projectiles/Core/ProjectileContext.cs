using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Core
{
    public sealed class ProjectileContext
    {
        public GameObject Owner { get; set; }
        public Transform Target { get; set; }
        
        public float Damage { get; set; }
        public float FiringRange { get; set; }
        public float Speed { get; set; }
        public float Size { get; set; }
        public float Duration { get; set; }
        public bool IsCritical { get; set; }
        
        public int BounceCount { get; set; }
        public float BounceRadius { get; set; }
        
        public float PierceCount { get; set; }
    }
}