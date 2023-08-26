using System;
using KthulhuWantsMe.Source.Gameplay.Interactions;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerInteractionsProcessor : MonoBehaviour
    {
        private IInteractionsManager _interactionsManager;

        [Inject]
        public void Construct(IInteractionsManager interactionsManager)
        {
            _interactionsManager = interactionsManager;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("EEE");
                _interactionsManager.ProcessInteraction();
            }
        }
    }
}