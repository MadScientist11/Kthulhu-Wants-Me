using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create YithConfiguration", fileName = "YithConfiguration", order = 0)]
    public class YithConfiguration : ScriptableObject
    {
        public GameObject Prefab;
        public float MaxHealth;
        public float BaseDamage;
        public float AttackCooldownTime;
        public float AttackDelay;
    }
}