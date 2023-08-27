using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Data
{
    
    [CreateAssetMenu(menuName = "Create InteractableData", fileName = "InteractableData", order = 0)]
    public class InteractableData : ScriptableObject
    {
        public string DisplayName;
        public string Description;
    }
}