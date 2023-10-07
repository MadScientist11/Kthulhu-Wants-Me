using System.Collections;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EmergeState : MonoBehaviour
    {
        public float EnemyHeight;
        public float EmergenceDuration;
        
        [SerializeField] private bool _hidePortalOnEmerge;
        
        private Portal _boundPortal;

        public void Init(Portal boundPortal)
        {
            _boundPortal = boundPortal;


            if (TryGetComponent(out RetreatState retreatState))
            {
                retreatState.Init(EnemyHeight, _boundPortal);
            }
        }

        public void Emerge(Vector3 from, Vector3 to) => 
            StartCoroutine(EmergeFromPortal(from, to));
        
        protected virtual void OnEmerge() {}
        protected virtual void OnEmerged() {}

        private IEnumerator EmergeFromPortal(Vector3 from, Vector3 to)
        {
            OnEmerge();
            Vector3 initialPosition = from;
            Vector3 targetPosition = to;

            float duration = EmergenceDuration;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
       
            if(_hidePortalOnEmerge)
                _boundPortal.gameObject.SetActive(false);
            
            OnEmerged();
        }
    }
}