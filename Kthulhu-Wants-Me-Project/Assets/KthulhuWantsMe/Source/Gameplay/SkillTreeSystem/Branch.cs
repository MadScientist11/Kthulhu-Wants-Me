using System;
using System.Collections.Generic;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [Serializable]
    public class Branch
    {
        public Guid InstanceId = Guid.NewGuid();
        public List<BranchStageTemplate> BranchStages;
    }
}