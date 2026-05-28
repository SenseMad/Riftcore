using System;
using Riftcore.Gameplay.Skills;
using Riftcore.Gameplay.Skills.Stats;
using UnityEngine;

namespace Riftcore.Gameplay.Stats
{
    [Serializable]
    public sealed class GameStatistics : IStatProvider
    {
        [field: SerializeField] public HealthStatistics HealthStatistics { get; set; }
        [field: SerializeField] public MovementStatistics MovementStatistics { get; set; }
        [field: SerializeField] public CombatStatistics CombatStatistics { get; set; }
        [field: SerializeField] public ProjectileStatistics ProjectileStatistics { get; set; }
        [field: SerializeField] public LevelStatistics LevelStatistics { get; set; }
        
        public GameStatistics(GameStatistics baseStatistics)
        {
            HealthStatistics = new HealthStatistics(baseStatistics.HealthStatistics);
            MovementStatistics = new MovementStatistics(baseStatistics.MovementStatistics);
            CombatStatistics = new CombatStatistics(baseStatistics.CombatStatistics);
            ProjectileStatistics = new ProjectileStatistics(baseStatistics.ProjectileStatistics);
            LevelStatistics = new LevelStatistics(baseStatistics.LevelStatistics);
        }

        public void ApplyModifier(StatModifierData statModifierData)
        {
            HealthStatistics.ApplyModifier(statModifierData);
            MovementStatistics.ApplyModifier(statModifierData);
            CombatStatistics.ApplyModifier(statModifierData);
            ProjectileStatistics.ApplyModifier(statModifierData);
            LevelStatistics.ApplyModifier(statModifierData);
        }

        public bool TryGetValue(StatType statType, out float value)
        {
            if (HealthStatistics.TryGetValue(statType, out value))
                return true;
            
            if (MovementStatistics.TryGetValue(statType, out value))
                return true;
            
            if (CombatStatistics.TryGetValue(statType, out value))
                return true;
            
            if (ProjectileStatistics.TryGetValue(statType, out value))
                return true;
            
            if (LevelStatistics.TryGetValue(statType, out value))
                return true;

            return false;
        }

        public bool TryGetStatValue(StatType statType, out float value)
        {
            return TryGetValue(statType, out value);
        }
    }
}