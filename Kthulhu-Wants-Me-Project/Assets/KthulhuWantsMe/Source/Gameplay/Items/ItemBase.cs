using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Items
{
    public interface IInteractable
    {
        void Interact();
    }
    public interface IPickable : IInteractable
    {
    }

    public class ItemBase : MonoBehaviour, IPickable
    {
        public void Interact()
        {
            
        }
    }
}