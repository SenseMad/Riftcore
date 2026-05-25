using System;
using System.Collections.Generic;
using Riftcore.Gameplay.Skills.Data;

namespace Riftcore.Gameplay.Skills.UI
{
    public interface ISkillChoicePanel
    {
        void Show(IReadOnlyList<SkillReward> rewards, Action<SkillReward> onChosen);
        void Hide();
    }
}