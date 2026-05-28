using System;
using System.Collections.Generic;
using Riftcore.Gameplay.Skills.Data;
using Riftcore.Gameplay.Skills.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Riftcore.Gameplay.Skills.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class SkillRewardButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _rarityText;
        
        [Header("Skills")]
        [SerializeField] private TMP_Text _skillLinePrefab;
        [SerializeField] private Transform _skillsContainer;

        private readonly List<TMP_Text> _skillLines = new();
        
        [Inject] private readonly IStatValueProvider _statValueProvider;
        
        public SkillReward SkillReward { get; private set; }
        public Button Button { get; private set; }
        
        public event Action<SkillReward> OnChosen;

        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        public void Initialize(SkillReward skillReward)
        {
            if (skillReward == null)
                return;
            
            _icon.sprite = skillReward.ItemData.Icon;
            _nameText.text = skillReward.ItemData.Name;
            _rarityText.text = skillReward.SkillRarity.ToString();

            _levelText.text = skillReward.IsNewItem ? "New" : $"LVL {skillReward.CurrentLevel}";
            
            CreateSkillLines(skillReward);
        }

        public void SetSkillReward(SkillReward skillReward, Action<SkillReward> onChosen)
        {
            SkillReward = skillReward;
            OnChosen = onChosen;
            
            Button.onClick.RemoveListener(OnRewardButton);
            Button.onClick.AddListener(OnRewardButton);
            
            Initialize(skillReward);
            
            Button.interactable = true;
        }

        private void CreateSkillLines(SkillReward skillReward)
        {
            ClearSkillLines();

            /*if (skillReward.IsNewItem)
            {
                //CreateSkillLine("New item");
                //return;
            }*/

            foreach (var skill in skillReward.SkillDatas)
            {
                float oldValue = 0f;

                if (!skillReward.IsNewItem)
                    _statValueProvider.TryGetValue(skillReward.ItemData, skill.StatType, out oldValue);
                
                CreateSkillLine(skill.GetFormattedChange(oldValue, skillReward.SkillRarity));
            }
        }

        private void CreateSkillLine(string text)
        {
            var line = Instantiate(_skillLinePrefab, _skillsContainer);
            line.text = text;
            
            _skillLines.Add(line);
        }

        private void ClearSkillLines()
        {
            foreach (var skillLine in _skillLines)
            {
                if (skillLine == null)
                    continue;
                
                Destroy(skillLine.gameObject);
            }
            
            _skillLines.Clear();
        }
        
        public void Clear()
        {
            SkillReward = null;
            OnChosen = null;
            
            Button.onClick.RemoveListener(OnRewardButton);
            
            _icon.sprite = null;
            _nameText.text = string.Empty;
            _levelText.text = string.Empty;
            _rarityText.text = string.Empty;
            
            ClearSkillLines();
        }

        private void OnRewardButton()
        {
            Button.interactable = false;
            
            Button.onClick.RemoveListener(OnRewardButton);
            
            OnChosen?.Invoke(SkillReward);
        }
    }
}