using System;
using UnityEngine;

namespace KthulhuWantsMe.Source
{
    [RequireComponent(typeof(Collider))]
    public class TriggerZone : MonoBehaviour
    {
        public event Action OnTrigger;
        
        private void OnValidate()
        {
            if (TryGetComponent(out Collider col)) 
                col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger?.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}
