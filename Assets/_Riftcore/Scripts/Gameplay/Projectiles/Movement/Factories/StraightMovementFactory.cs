using Riftcore.Gameplay.Projectiles.Movement.Implementations;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Movement.Factories
{
    [CreateAssetMenu(fileName = "StraightMovementFactory", menuName = "Riftcore/Projectile/Data/Movement/StraightMovementFactory")]
    public sealed class StraightMovementFactory : ProjectileMovementFactory
    {
        public override IProjectileMovement Create()
        {
            return new StraightMovement();
        }
    }
}