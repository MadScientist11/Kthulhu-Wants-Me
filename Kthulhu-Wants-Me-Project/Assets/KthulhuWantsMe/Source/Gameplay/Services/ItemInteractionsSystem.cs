using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public class ItemInteractionsSystem
    {
        public void OnRemoveItem(IPickable item)
        {
            if (item is IConsumable { RemainingUses: 0 } consumableItem)
            {
                //consumableItem.DestroyConsumable();
            }
        }
    }
}