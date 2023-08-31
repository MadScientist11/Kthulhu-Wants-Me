using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create MinionConfiguration", fileName = "MinionConfiguration", order = 0)]
    public class MinionConfiguration : ScriptableObject
    {
        public GameObject MinionPrefab;
    }
}