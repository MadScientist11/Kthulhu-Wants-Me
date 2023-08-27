using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class ConsumableItem : PickableItem, IConsumable
    {
        public virtual int MaxUses { get; } = 1;
        public int RemainingUses { get; set; }

        private void Start()
        {
            RemainingUses = MaxUses;
        }

        public void Consume()
        {
            RemainingUses--;
            Debug.Log("Consume");
            if (RemainingUses == 0)
            {
                DestroyConsumable();
            }
        }

        private void DestroyConsumable()
        {
            _inventorySystem.RemoveItem(this);
            Destroy(gameObject);
        }
    }
}