using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Data
{
    
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create PickableData", fileName = "PickableData", order = 0)]
    public class PickableData : InteractableData
    {
        public Vector3 ItemInHandPosition;
        public Quaternion ItemInHandRotation;
    }
}