using System;
using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [Serializable]
    public class Branch
    {
        public Guid InstanceId = Guid.NewGuid();
        public string BranchName;
        public List<BranchStageTemplate> BranchStages;
    }
}