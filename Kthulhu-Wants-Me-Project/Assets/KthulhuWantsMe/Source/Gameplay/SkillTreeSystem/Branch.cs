using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [Serializable]
    public class Branch
    {
        public List<UpgradeData> Upgrades;
        public SkillId SkillId;
    }
}