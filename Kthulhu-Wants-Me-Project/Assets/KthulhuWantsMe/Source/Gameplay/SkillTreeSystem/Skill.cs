using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    [CreateAssetMenu(menuName = "Create Skill", fileName = "Skill", order = 0)]
    public class Skill : ScriptableObject
    {
        public SkillId SkillId;
        public string Name;
        public string Description;
        public Sprite Sprite;
    }
}