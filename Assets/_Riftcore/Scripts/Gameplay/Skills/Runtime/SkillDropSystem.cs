using System;
using System.Collections.Generic;
using Riftcore.Core.GameState;
using Riftcore.Gameplay.Experience;
using Riftcore.Gameplay.Experience.Core;
using Riftcore.Gameplay.Skills.Data;
using Riftcore.Gameplay.Skills.UI;

namespace Riftcore.Gameplay.Skills.Runtime
{
    public sealed class SkillDropSystem : IDisposable
    {
        private readonly ExperienceService _experienceService;
        private readonly SkillSelectionService _skillSelectionService;
        private readonly SkillRewardApplier _skillRewardApplier;
        private readonly ISkillChoicePanel _skillChoicePanel;
        private readonly GameplayLockService _gameplayLockService;

        public SkillDropSystem(ExperienceService experienceService, SkillSelectionService skillSelectionService, 
            SkillRewardApplier skillRewardApplier, ISkillChoicePanel skillChoicePanel, GameplayLockService gameplayLockService)
        {
            _experienceService = experienceService;
            _skillSelectionService = skillSelectionService;
            _skillRewardApplier = skillRewardApplier;
            _skillChoicePanel = skillChoicePanel;
            _gameplayLockService = gameplayLockService;
            
            _experienceService.OnLevelUp += OnLevelUp;
        }

        public void Dispose()
        {
            _experienceService.OnLevelUp -= OnLevelUp;
        }
        
        private void OnLevelUp(int level)
        {
            List<SkillReward> rewards = _skillSelectionService.GenerateChoices(3);
            
            _gameplayLockService.Lock(this);
            
            _skillChoicePanel.Show(rewards, OnSkillChosen);
        }

        private void OnSkillChosen(SkillReward reward)
        {
            _skillRewardApplier.Apply(reward);
            
            _skillChoicePanel.Hide();
            
            _gameplayLockService.Unlock(this);
        }
    }
}