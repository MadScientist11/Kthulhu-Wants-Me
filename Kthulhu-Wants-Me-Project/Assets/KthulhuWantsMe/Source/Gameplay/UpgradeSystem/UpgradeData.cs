using System;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using Sirenix.OdinInspector;

namespace KthulhuWantsMe.Source.Gameplay.UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
        public string UpgradeTitle;
        public string UpgradeText;
        public UpgradeType UpgradeType;

        [ShowIf("UpgradeType", UpgradeType.StatUpgrade)]
        [EnumPaging]
        public StatType StatType;
        
        [EnumToggleButtons]        
        public UpgradeValueType UpgradeValueType;
        public float Value;
    }
}