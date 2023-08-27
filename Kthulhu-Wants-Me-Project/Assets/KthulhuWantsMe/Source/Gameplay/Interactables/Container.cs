using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables
{
    public class Container : MonoBehaviour, IInteractable
    {
        public Transform Transform => transform;
        public bool Interact()
        {
            return false;
        }
    }
}
