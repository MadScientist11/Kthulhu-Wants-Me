using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [CreateAssetMenu(menuName = "Create BranchTemplate", fileName = "BranchTemplate", order = 0)]
    public class BranchTemplate : ScriptableObject
    {
        public Branch Branch;
    }
}