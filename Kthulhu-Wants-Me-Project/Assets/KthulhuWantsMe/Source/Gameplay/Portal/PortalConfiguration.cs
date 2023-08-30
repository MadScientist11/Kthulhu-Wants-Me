using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Portal
{
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create PortalConfiguration", fileName = "PortalConfiguration", order = 0)]
    public class PortalConfiguration : ScriptableObject
    {
        public PortalEnemySpawner PortalPrefab;
    }
}