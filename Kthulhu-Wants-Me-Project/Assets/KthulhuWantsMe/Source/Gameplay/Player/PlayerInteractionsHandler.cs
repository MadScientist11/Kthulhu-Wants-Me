using System;
using KthulhuWantsMe.Source.Gameplay.Interactions;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerInteractionsHandler : MonoBehaviour
    {
        private IInteractionsManager _interactionsManager;

        [Inject]
        public void Construct(IInteractionsManager interactionsManager)
        {
            _interactionsManager = interactionsManager;
        }

        private void Interact()
        {
            
        }
    }
}