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
        public StatType StatType;

        public UpgradeValueType UpgradeValueType;
        public float Value;
    }
}