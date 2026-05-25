using UnityEngine;

namespace Riftcore.Gameplay.Experience.Data
{
    [CreateAssetMenu(fileName = "ExperienceData", menuName = "Riftcore/Experience/Data/ExperienceData")]
    public sealed class ExperienceData : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int BaseExperience { get; private set; }
        [field: SerializeField, Min(0)] public float GrowthFactor { get; private set; }
        
        public int GetRequiredExperience(int level)
        {
            level = Mathf.Max(1, level);
            
            int experience = Mathf.CeilToInt(BaseExperience + ((level - 1) * 50) + Mathf.Pow((level - 1), GrowthFactor) * 50);
            
            return Mathf.Max(1, experience);
        }
    }
}