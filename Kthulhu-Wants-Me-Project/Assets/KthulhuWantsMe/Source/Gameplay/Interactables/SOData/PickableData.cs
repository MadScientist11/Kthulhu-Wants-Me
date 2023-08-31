using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.SOData
{
    
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create PickableData", fileName = "PickableData", order = 0)]
    public class PickableData : InteractableData
    {
        public Vector3 ItemInHandPosition;
        public Quaternion ItemInHandRotation;
    }
}