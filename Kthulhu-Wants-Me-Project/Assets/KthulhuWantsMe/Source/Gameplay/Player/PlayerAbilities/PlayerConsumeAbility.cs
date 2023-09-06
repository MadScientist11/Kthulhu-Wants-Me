using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerConsumeAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private PlayerHealth _playerHealth;
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IGameFactory gameFactory,IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }

        public void Consume(IConsumable consumable)
        {
            consumable.RemainingUses--;
            if (consumable.RemainingUses == 0)
            {
                
            }
            consumable.RespondTo(this);
        }
    }
}