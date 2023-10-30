using System;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using Sirenix.OdinInspector;

namespace KthulhuWantsMe.Source.Gameplay.UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
     
        public UpgradeType UpgradeType;

        [ShowIf(nameof(UpgradeType), UpgradeType.StatUpgrade)]
        [EnumPaging]
        public StatType StatType;
        
        [ShowIf(nameof(UpgradeType), UpgradeType.SkillAcquirement)]
        public SkillId SkillId;
        
        [ShowIf(nameof(UpgradeType), UpgradeType.StatUpgrade)]
        [EnumToggleButtons]        
        public UpgradeValueType UpgradeValueType;
        
        [HideIf(nameof(UpgradeType), UpgradeType.SkillAcquirement)]
        public float Value;
    }
}