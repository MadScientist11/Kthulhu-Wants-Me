using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerInteractionAbility : MonoBehaviour, IAbility
    {
        public IAbility CurrentInteractionAbility { get; private set; }
        
        [SerializeField] private PlayerHighlightAbility _playerHighlightAbility;
        [SerializeField] private PlayerInventoryAbility _playerInventoryAbility;
        
        [SerializeField] private TriggerObserver _autoInteractionZone;

        private bool _prohibitBuffsUsage;
        
        private IInputService _inputService;
        private IBuffDebuffService _buffDebuffService; 

        [Inject]
        public void Construct(IInputService inputService, IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
            _inputService = inputService;

            _inputService.GameplayScenario.Interact += OnInteractButtonPressed;
            _autoInteractionZone.TriggerEnter += HandleProximityBasedInteractions;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Interact -= OnInteractButtonPressed;
            _autoInteractionZone.TriggerEnter -= HandleProximityBasedInteractions;
        }

        public void ApplyEffectToPlayer(IBuffDebuff effect)
        {
            _buffDebuffService.ApplyEffect(effect, GetComponent<EntityBuffDebuffContainer>());
        }
        public void ApplyBuffsUsageRestriction()
        {
            _prohibitBuffsUsage = true;
        }
        
        
        public void CancelBuffsUsageRestriction()
        {
            _prohibitBuffsUsage = false;
        }

        private void HandleProximityBasedInteractions(Collider interaction)
        {
            if(!interaction.TryGetComponent(out IAutoInteractable autoInteractable))
                return;

            if (autoInteractable is BuffItem && !_prohibitBuffsUsage)
            {
               autoInteractable.RespondTo(this);
            }
        }


        private void OnInteractButtonPressed()
        {
            if (_playerHighlightAbility.MouseHoverInteractable != null)
            {
                ProcessHighlightedInteractable(_playerHighlightAbility.MouseHoverInteractable);
                return;
            }

            if (_playerHighlightAbility.InteractablesInZone.Count > 0)
            {
                ProcessHighlightedInteractable(_playerHighlightAbility.InteractablesInZone[0]);
            }
            // PlayerItemInteraction
            // PlayerObjectInteraction - doors, explorable objects
            // //PlayerNPCInteraction
        }

        private void ProcessHighlightedInteractable(IInteractable interactable)
        {
            if (interactable is IPickable item)
            {
                CurrentInteractionAbility = _playerInventoryAbility;
                _playerInventoryAbility.PickUpItem(item);
                return;
            }
            
            interactable.RespondTo(this);
        }
    }
}