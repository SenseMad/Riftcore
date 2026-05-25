using Riftcore.Gameplay.Experience.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Experience.UI
{
    public sealed class ExperienceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _experienceText;

        [Inject] private readonly ExperienceService _experienceService;

        private void OnEnable()
        {
            _experienceService.OnLevelUp += OnLevelUp;
            _experienceService.OnExperienceChanged += OnExperienceChanged;
            
            OnLevelUp(_experienceService.CurrentLevel);
            OnExperienceChanged(_experienceService.CurrentExperience, _experienceService.RequiredExperience);
        }

        private void Start()
        {
            OnLevelUp(_experienceService.CurrentLevel);
            OnExperienceChanged(_experienceService.CurrentExperience, _experienceService.RequiredExperience);
        }

        private void OnDisable()
        {
            _experienceService.OnLevelUp -= OnLevelUp;
            _experienceService.OnExperienceChanged -= OnExperienceChanged;
        }
        
        private void OnLevelUp(int level)
        {
            _levelText.text = $"Level: {level}";
        }

        private void OnExperienceChanged(int currentExperience, int requiredExperience)
        {
            _experienceText.text = $"{currentExperience} / {requiredExperience}";
        }
    }
}