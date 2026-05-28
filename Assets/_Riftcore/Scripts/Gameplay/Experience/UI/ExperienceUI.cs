using System.Collections;
using Riftcore.Gameplay.Experience.Core;
using Riftcore.Gameplay.Skills.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Riftcore.Gameplay.Experience.UI
{
    public sealed class ExperienceUI : MonoBehaviour
    {
        [SerializeField] private Image _experienceBar;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _experienceText;

        [Inject] private readonly ExperienceService _experienceService;
        [Inject] private readonly SkillSelectionService _skillSelectionService;
        
        private float _maxExperienceFillWidth;
        
        private float _displayExperience;
        private int _displayRequiredExperience;
        
        private float _targetExperience;
        private int _targetRequiredExperience;
        
        private bool _isWaitingSkillSelection;
        
        private Coroutine _animateExperience;

        private void Awake()
        {
            _maxExperienceFillWidth = _experienceBar.rectTransform.sizeDelta.x;
        }

        private void OnEnable()
        {
            InitializeView();
            
            _experienceService.OnLevelUp += OnLevelUp;
            _experienceService.OnExperienceChanged += OnExperienceChanged;
            _skillSelectionService.OnSkillSelected += ContinueAfterSkillSelected;
        }

        private void OnDisable()
        {
            _experienceService.OnLevelUp -= OnLevelUp;
            _experienceService.OnExperienceChanged -= OnExperienceChanged;
            _skillSelectionService.OnSkillSelected -= ContinueAfterSkillSelected;

            if (_animateExperience != null)
            {
                StopCoroutine(_animateExperience);
                _animateExperience = null;
            }
        }
        
        private void InitializeView()
        {
            _displayRequiredExperience = _experienceService.RequiredExperience;
            _targetRequiredExperience = _experienceService.RequiredExperience;

            _displayExperience = _experienceService.CurrentExperience;
            _targetExperience = _experienceService.CurrentExperience;

            _isWaitingSkillSelection = false;

            UpdateView(_displayExperience, _displayRequiredExperience);
        }
        
        private void ContinueAfterSkillSelected()
        {
            _isWaitingSkillSelection = false;

            if (_animateExperience != null)
                StopCoroutine(_animateExperience);

            _displayExperience = 0;
            _displayRequiredExperience = _targetRequiredExperience;

            UpdateView(_displayExperience, _displayRequiredExperience);

            _animateExperience = StartCoroutine(AnimateExperience());
        }
        
        private void OnLevelUp(int level)
        {
            _levelText.text = $"LVL: {level}";

            if (_animateExperience != null)
            {
                StopCoroutine(_animateExperience);
                _animateExperience = null;
            }

            _displayExperience = _displayRequiredExperience;
            UpdateView(_displayExperience, _displayRequiredExperience);

            _isWaitingSkillSelection = true;
        }
        
        private void OnExperienceChanged(int currentExperience, int requiredExperience)
        {
            if (_isWaitingSkillSelection)
            {
                _targetExperience = currentExperience;
                _targetRequiredExperience = requiredExperience;
                return;
            }

            bool isLevelUp = requiredExperience != _displayRequiredExperience;

            if (_animateExperience != null)
            {
                StopCoroutine(_animateExperience);
                _animateExperience = null;
            }

            if (isLevelUp)
            {
                _displayExperience = _displayRequiredExperience;
                UpdateView(_displayExperience, _displayRequiredExperience);

                _targetExperience = currentExperience;
                _targetRequiredExperience = requiredExperience;

                _isWaitingSkillSelection = true;
            }
            else
            {
                _targetExperience = currentExperience;
                _animateExperience = StartCoroutine(AnimateExperience());
            }
        }
        
        private void UpdateView(float experience, float requiredExperience)
        {
            float width = _maxExperienceFillWidth * Mathf.Clamp01(experience / requiredExperience);
            _experienceBar.rectTransform.sizeDelta = new Vector2(width, _experienceBar.rectTransform.sizeDelta.y);
        }
        
        private IEnumerator AnimateExperience()
        {
            const float speed = 75f;

            while (!Mathf.Approximately(_displayExperience, _targetExperience))
            {
                _displayExperience = Mathf.MoveTowards(
                    _displayExperience,
                    _targetExperience,
                    speed * Time.deltaTime
                );

                UpdateView(_displayExperience, _displayRequiredExperience);

                yield return null;
            }

            _animateExperience = null;
        }
    }
}