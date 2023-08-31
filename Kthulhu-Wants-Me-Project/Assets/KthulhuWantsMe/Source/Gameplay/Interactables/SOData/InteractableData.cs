using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.SOData
{
    
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create InteractableData", fileName = "InteractableData", order = 0)]
    public class InteractableData : ScriptableObject
    {
        public string DisplayName;
        public string Description;
    }
}