using System;
using System.Collections.Generic;
using Riftcore.Gameplay.Skills.Data;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Skills.UI
{
    public sealed class SkillChoicePanel : MonoBehaviour, ISkillChoicePanel
    {
        [SerializeField] private GameObject _ownerObject;
        [SerializeField] private SkillRewardButton _skillRewardButtonPrefab;
        [SerializeField] private Transform _skillContainer;
        
        [Inject] private readonly DiContainer _container;
        
        private readonly List<SkillRewardButton> _skillRewardButtons = new();

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                var skillRewardButton = _container.InstantiatePrefabForComponent<SkillRewardButton>(_skillRewardButtonPrefab, _skillContainer);
                skillRewardButton.gameObject.SetActive(false);
                
                _skillRewardButtons.Add(skillRewardButton);
            }

            Hide();
        }

        public void Show(IReadOnlyList<SkillReward> rewards, Action<SkillReward> onChosen)
        {
            _ownerObject.SetActive(true);
            
            CreateSkillRewards(rewards, onChosen);
        }

        public void Hide()
        {
            foreach (var skillRewardButton in _skillRewardButtons)
            {
                skillRewardButton.Clear();
                skillRewardButton.gameObject.SetActive(false);
            }
            
            _ownerObject.SetActive(false);
        }

        private void CreateSkillRewards(IReadOnlyList<SkillReward> rewards, Action<SkillReward> onChosen)
        {
            for (int i = 0; i < Mathf.Min(rewards.Count, _skillRewardButtons.Count); i++)
            {
                var reward = rewards[i];
                var skillRewardButton = _skillRewardButtons[i];
                
                skillRewardButton.gameObject.SetActive(true);
                skillRewardButton.SetSkillReward(reward, onChosen);
            }
        }
    }
}