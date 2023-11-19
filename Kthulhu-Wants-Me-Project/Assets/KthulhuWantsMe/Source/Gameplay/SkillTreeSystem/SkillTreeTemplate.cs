using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [CreateAssetMenu(menuName = "Create SkillTreeTemplate", fileName = "SkillTreeTemplate", order = 0)]
    public class SkillTreeTemplate : ScriptableObject
    {
        public SkillTree SkillTree;
    }
}