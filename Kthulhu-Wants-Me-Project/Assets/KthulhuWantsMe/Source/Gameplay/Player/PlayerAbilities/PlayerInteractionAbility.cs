using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
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
        private IPlayerStats _playerStats;

        [Inject]
        public void Construct(IInputService inputService, IPlayerStats playerStats)
        {
            _playerStats = playerStats;
            _inputService = inputService;

            _inputService.GameplayScenario.Interact += OnInteractButtonPressed;
            _autoInteractionZone.TriggerEnter += HandleProximityBasedInteractions;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Interact -= OnInteractButtonPressed;
            _autoInteractionZone.TriggerEnter -= HandleProximityBasedInteractions;
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

            if (autoInteractable is IBuff buffItem && !_prohibitBuffsUsage)
            {
               _playerStats.ApplyBuff(buffItem);
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