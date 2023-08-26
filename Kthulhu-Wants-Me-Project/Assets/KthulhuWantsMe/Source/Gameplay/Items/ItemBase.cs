using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Items
{
    public interface IInteractable
    {
        Transform Transform { get; }

        void Interact();
    }
    public interface IPickable : IInteractable
    {
        bool Equipped { get; set; }
    }

    public class ItemBase : MonoBehaviour, IPickable
    {
        public bool Equipped { get; set; }
        public Transform Transform => transform;


        public void Interact()
        {
            
        }
    }
}