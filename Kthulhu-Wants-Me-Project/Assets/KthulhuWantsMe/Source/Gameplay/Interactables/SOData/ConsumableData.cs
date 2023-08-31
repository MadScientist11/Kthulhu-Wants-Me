using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.SOData
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create ConsumableData", fileName = "ConsumableData", order = 0)]
    public class ConsumableData : PickableData
    {
        public int MaxUses;
    }
}