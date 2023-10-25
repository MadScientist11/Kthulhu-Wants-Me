using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [CreateAssetMenu(menuName = "Create BranchStageTemplate", fileName = "BranchStageTemplate", order = 0)]
    public class BranchStageTemplate : ScriptableObject
    {
        public BranchStage BranchStage;
    }
}