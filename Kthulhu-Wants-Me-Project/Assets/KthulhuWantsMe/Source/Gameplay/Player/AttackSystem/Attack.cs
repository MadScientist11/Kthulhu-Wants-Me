using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create Attack", fileName = "Attack", order = 0)]
    public class Attack : ScriptableObject
    {
        public AnimatorOverrideController AttackOverrideController;
        public float Damage;

    }
}