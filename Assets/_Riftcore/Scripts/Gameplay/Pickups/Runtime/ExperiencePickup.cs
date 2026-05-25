using Riftcore.Gameplay.Experience;
using Riftcore.Gameplay.Experience.Core;
using Riftcore.Gameplay.Pickups.Core;
using Riftcore.Gameplay.Players.Core;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Pickups.Runtime
{
    public sealed class ExperiencePickup : BasePickup
    {
        [Inject] private readonly ExperienceService _experienceService;
        
        [field: SerializeField] public int AmountExperience { get; private set; }

        public void SetAmountExperience(int amountExperience)
        {
            if (amountExperience < 0)
                return;
            
            AmountExperience = amountExperience;
        }

        public override void OnPickup(Player player)
        {
            _experienceService.AddExperience(Mathf.CeilToInt(AmountExperience * player.GameStatistics.LevelStatistics.MultiplierExperience));
            _pickupManager.Unregister(this);
        }
    }
}