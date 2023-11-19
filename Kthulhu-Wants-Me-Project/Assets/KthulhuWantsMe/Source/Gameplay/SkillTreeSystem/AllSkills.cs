using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SkillTreeSystem
{
    public class AllSkills
    {
        private Dictionary<SkillId, Skill> _skillsData;
        public AllSkills()
        {
            _skillsData = Resources.LoadAll<Skill>("Skills/").ToDictionary(skill => skill.SkillId, skill => skill);
        }
        
        public Skill this[SkillId skillId] => 
            _skillsData[skillId];
    }
}