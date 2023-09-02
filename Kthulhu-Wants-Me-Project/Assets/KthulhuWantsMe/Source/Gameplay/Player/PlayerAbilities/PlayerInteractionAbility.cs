﻿using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerInteractionAbility : MonoBehaviour, IAbility
    {
        public IAbility CurrentInteractionAbility { get; private set; }
        
        [SerializeField] private PlayerHighlightAbility _playerHighlightAbility;
        [SerializeField] private PlayerInventoryAbility _playerInventoryAbility;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_playerHighlightAbility.MouseHoverInteractable != null)
                {
                    Debug.Log(_playerHighlightAbility.MouseHoverInteractable);
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