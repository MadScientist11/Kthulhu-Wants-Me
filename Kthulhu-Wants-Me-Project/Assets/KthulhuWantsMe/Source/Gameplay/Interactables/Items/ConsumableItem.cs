using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using NaughtyAttributes;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public sealed class ConsumableItem : PickableItem, IConsumable
    {
        [field: SerializeField] public ConsumableData ConsumableData { get; set; }
        
        public override PickableData ItemData => ConsumableData;
      
        public int RemainingUses { get; set; }

        private void Start()
        {
            RemainingUses = ConsumableData.MaxUses;
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
            Destroy(gameObject);
        }
    }
}