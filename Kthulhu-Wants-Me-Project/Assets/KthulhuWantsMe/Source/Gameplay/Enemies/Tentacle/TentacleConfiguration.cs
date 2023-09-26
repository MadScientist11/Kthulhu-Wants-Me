using KthulhuWantsMe.Source.Gameplay.Spell;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "TentacleConfiguration", fileName = "TentacleConfiguration",
        order = 0)]
    public class TentacleConfiguration : EnemyConfiguration
    {
        public float StunWearOffTime;
        public float TentacleGrabDamage;
        public float GrabAbilityChance;

        public float AggroRange;
    }
}