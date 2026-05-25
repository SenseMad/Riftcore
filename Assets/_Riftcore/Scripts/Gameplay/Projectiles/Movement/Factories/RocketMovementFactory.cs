using Riftcore.Gameplay.Projectiles.Movement.Implementations;
using Riftcore.Gameplay.Projectiles.Movement.Interfaces;
using UnityEngine;

namespace Riftcore.Gameplay.Projectiles.Movement.Factories
{
    [CreateAssetMenu(fileName = "RocketMovementFactory", menuName = "Riftcore/Projectile/Data/Movement/RocketMovementFactory")]
    public sealed class RocketMovementFactory : ProjectileMovementFactory
    {
        public override IProjectileMovement Create()
        {
            return new RocketMovement();
        }
    }
}