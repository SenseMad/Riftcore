using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Riftcore.Gameplay.Skills.Data
{
    [CreateAssetMenu(fileName = "RarityDatabase", menuName = "Riftcore/Installers/RarityDatabase")]
    public sealed class RarityDatabase : ScriptableObjectInstaller<RarityDatabase>
    {
        [field: SerializeField] public List<RarityChanceData> RarityChanceDatas { get; private set; }
        
        public override void InstallBindings()
        {
            Container.Bind<RarityDatabase>().FromInstance(this).AsSingle();
        }

        public SkillRarity GetRandomRarity(float luck)
        {
            float totalWeight = 0f;

            foreach (var rarityChanceData in RarityChanceDatas)
                totalWeight += GetWeightWithLuck(rarityChanceData, luck);
            
            if (totalWeight <= 0f)
                return SkillRarity.Common;
            
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var rarityChanceData in RarityChanceDatas)
            {
                currentWeight += GetWeightWithLuck(rarityChanceData, luck);
                
                if (randomValue <= currentWeight)
                    return rarityChanceData.SkillRarity;
            }
            
            return SkillRarity.Common;
        }

        public float GetWeightWithLuck(RarityChanceData rarityChanceData, float luck)
        {
            float weight = rarityChanceData.Weight;

            // Добавлять удачу только для (Rare, Epic, Legendary)
            if (rarityChanceData.SkillRarity is SkillRarity.Rare or SkillRarity.Epic or SkillRarity.Legendary)
                weight += luck;

            return Mathf.Max(0, weight);
        }
    }
}