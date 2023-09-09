using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create CyaeghaConfiguration", fileName = "MinionConfiguration", order = 0)]
    public class CyaeghaConfiguration : ScriptableObject
    {
        public GameObject Prefab;
        public float MaxHealth;
        public float BaseDamage;
        public float AttackCooldownTime;
        public float AttackDelay;
    }
}