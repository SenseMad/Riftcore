using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Movement.Factories
{
    public abstract class ProjectileMovementFactory : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        
        public abstract IProjectileMovement Create();
    }
}