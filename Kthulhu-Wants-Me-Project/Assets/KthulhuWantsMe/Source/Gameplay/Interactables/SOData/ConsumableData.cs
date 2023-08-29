using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Data
{
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create ConsumableData", fileName = "ConsumableData", order = 0)]
    public class ConsumableData : PickableData
    {
        public int MaxUses;
    }
}