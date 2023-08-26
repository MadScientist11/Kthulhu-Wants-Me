using KthulhuWantsMe.Source.Gameplay.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] private Transform _itemParent;
        
        public void PickUp(IPickable pickable)
        {
            Debug.Log(pickable);
            pickable.Transform.GetComponent<Rigidbody>().isKinematic = true;
            pickable.Transform.SetParent(_itemParent);
            pickable.Transform.localPosition = Vector3.zero;
            pickable.Equipped = true;
        }
        
        public void SwitchItem(IPickable newItem, IPickable previousItem)
        {
            Debug.Log(newItem);
            newItem?.Transform.gameObject.SetActive(true);
            previousItem?.Transform.gameObject.SetActive(false);
        }
        public void ThrowAway(IPickable pickable)
        {
            pickable.Equipped = false;

            pickable.Transform.SetParent(null);
            pickable.Transform.GetComponent<Rigidbody>().isKinematic = false;
            pickable.Transform.GetComponent<Rigidbody>().AddForce(transform.forward*150);
        }
    }
}