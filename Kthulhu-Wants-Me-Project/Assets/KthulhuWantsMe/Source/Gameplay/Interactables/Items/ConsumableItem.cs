using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
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

        public void RespondTo(PlayerConsumeAbility ability)
        {
            //destroy
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