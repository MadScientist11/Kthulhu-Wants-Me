using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create PortalConfiguration", fileName = "PortalConfiguration", order = 0)]
    public class PortalConfiguration : ScriptableObject
    {
        public PortalEnemySpawner PortalPrefab;
        public PortalsLogic.Portal TentaclePortalPrefab;
    }
}