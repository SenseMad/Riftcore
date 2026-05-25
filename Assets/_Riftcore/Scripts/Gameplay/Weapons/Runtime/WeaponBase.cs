using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using Riftcore.Gameplay.Weapons.Data;

namespace Riftcore.Gameplay.Weapons.Runtime
{
    public abstract class Weapon
    {
        public abstract WeaponData WeaponData { get; }
        
        public abstract void Tick();

        public abstract void ApplyModifier(StatModifierData statModifierData);
        public abstract bool TryGetStatValue(StatType statType, out float value);
    }
}