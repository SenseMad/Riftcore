using System.Collections.Generic;
using Riftcore.Gameplay.Skills.Data;
using UnityEngine;

namespace Riftcore.Gameplay.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Riftcore/Inventory/Data/ItemData")]
    public sealed class ItemData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemCategory ItemCategory { get; private set; }
        [field: SerializeField, Min(1)] public int MaxLevel { get; private set; }
        
        [field: Header("Skills")]
        [field: SerializeField, Min(1)] public int MinNumberDropSkills { get; private set; }
        [field: SerializeField, Min(1)] public int MaxNumberDropSkills { get; private set; }
        [field: SerializeField] public List<SkillData> AvailableSkills { get; private set; }
    }
}