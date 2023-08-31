using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Portal
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create PortalConfiguration", fileName = "PortalConfiguration", order = 0)]
    public class PortalConfiguration : ScriptableObject
    {
        public PortalEnemySpawner PortalPrefab;
    }
}