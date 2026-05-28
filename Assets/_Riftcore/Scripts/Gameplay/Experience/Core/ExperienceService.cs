using System;
using Riftcore.Gameplay.Experience.Data;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Experience.Core
{
    public sealed class ExperienceService : IInitializable
    {
        [Inject] private readonly ExperienceData _experienceData;
        
        public int CurrentExperience { get; private set; }
        public int CurrentLevel { get; private set; }
        
        public bool IsLevelUpPending { get; private set; }

        public int RequiredExperience => _experienceData.GetRequiredExperience(CurrentLevel);
        
        public event Action<int, int> OnExperienceChanged;
        public event Action<int> OnLevelUp;
        
        public void Initialize()
        {
            CurrentExperience = 0;
            CurrentLevel = 1;
            IsLevelUpPending = false;
            
            OnExperienceChanged?.Invoke(CurrentExperience, RequiredExperience);
        }

        public void ForceLevelUp()
        {
            CurrentLevel++;
            IsLevelUpPending = true;
            OnLevelUp?.Invoke(CurrentLevel);
        }

        public void CompleteLevelUpSelection()
        {
            if (!IsLevelUpPending)
                return;

            IsLevelUpPending = false;
            
            TryLevelUp();
            
            OnExperienceChanged?.Invoke(CurrentExperience, RequiredExperience);
        }

        public void AddExperience(int amount)
        {
            if (amount <= 0)
                return;
            
            CurrentExperience += amount;

            TryLevelUp();
            
            OnExperienceChanged?.Invoke(CurrentExperience, RequiredExperience);
        }

        private void TryLevelUp()
        {
            if (IsLevelUpPending)
                return;

            int requiredExperience = RequiredExperience;
            if (CurrentExperience < requiredExperience)
                return;

            CurrentExperience -= requiredExperience;

            CurrentLevel++;
            IsLevelUpPending = true;

            OnLevelUp?.Invoke(CurrentLevel);
        }
    }
}