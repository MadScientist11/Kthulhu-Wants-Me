using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class RetreatState : MonoBehaviour
    {
        public float RetreatDuration;

        [SerializeField] private Health _enemyHealth;
        
        protected Vector3 InitialPoint { get; private set; }
        
        private Portal _boundPortal;
        private float _height;

        public void Init(float height, Portal boundPortal)
        {
            _height = height;
            _boundPortal = boundPortal;
            _enemyHealth.Died += Retreat;
        }

        private void OnDestroy()
        {
            _enemyHealth.Died -= Retreat;
        }

        public void Retreat() => 
            StartCoroutine(RetreatToPortal(transform.position.AddY(-_height)));

        protected virtual void OnRetreat() {}

        protected virtual void OnRetreated() {}

        private IEnumerator RetreatToPortal(Vector3 to)
        {
            OnRetreat();
            Vector3 initialPosition = transform.position;
            InitialPoint = initialPosition;
            Vector3 targetPosition = to;

            float duration = RetreatDuration;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            if(_boundPortal != null)
                _boundPortal.ClosePortal();
            
            OnRetreated();
        }
    }
}