using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Data
{
    
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create InteractableData", fileName = "InteractableData", order = 0)]
    public class InteractableData : ScriptableObject
    {
        public string DisplayName;
        public string Description;
    }
}