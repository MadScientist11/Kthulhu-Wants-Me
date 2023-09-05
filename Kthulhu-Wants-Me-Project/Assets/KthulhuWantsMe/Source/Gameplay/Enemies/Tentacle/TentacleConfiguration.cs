using KthulhuWantsMe.Source.Gameplay.Spell;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "TentacleConfiguration", fileName = "TentacleConfiguration",
        order = 0)]
    public class TentacleConfiguration : ScriptableObject
    {
        public GameObject TentaclePrefab;
        public MinionsSpawnSpell MinionsSpawnSpell;

        public float MaxHealth;

        public float BaseDamage;
        public float AttackRadius;
        public float AttackEffectiveDistance;
        public float AttackCooldown;
        public float StunWearOffTime;
        public float TentacleGrabDamage;


        public float GrabAbilityChance;
        public float SpellActivationTime;

     
    }
}