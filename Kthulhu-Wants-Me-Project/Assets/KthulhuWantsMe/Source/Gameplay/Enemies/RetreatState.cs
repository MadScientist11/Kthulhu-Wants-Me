using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class RetreatState : MonoBehaviour
    {
        public float RetreatDuration;
        public float Height;
        
        [SerializeField] private Health _enemyHealth;

        protected Vector3 InitialPoint { get; private set; }

        private Portal _boundPortal;
        private float _height = 2;

        public void Init(float height, Portal boundPortal)
        {
            _height = height;
            _boundPortal = boundPortal;
            _enemyHealth.Died += RetreatDefeated;
        }

        private void OnDestroy()
        {
            _enemyHealth.Died -= RetreatDefeated;
        }

        public void RetreatDefeated() =>
            StartCoroutine(RetreatToPortalDefeated(transform.position.AddY(-Height)));

        public void Retreat()
        {
            StartCoroutine(RetreatToPortal(transform.position.AddY(-Height)));
        }

        protected virtual void OnRetreat()
        {
        }

        protected virtual void OnRetreated()
        {
        }
        
        protected virtual void OnRetreatDefeated()
        {
        }

        protected virtual void OnRetreatedDefeated()
        {
        }
        

        private IEnumerator RetreatToPortal(Vector3 to)
        {
            _boundPortal.gameObject.SetActive(true);
            _boundPortal.transform.position = transform.position;
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

            if (_boundPortal != null)
                _boundPortal.ClosePortal();

            OnRetreated();
        }
        
        private IEnumerator RetreatToPortalDefeated(Vector3 to)
        {
            OnRetreatDefeated();
            
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

            if (_boundPortal != null)
                _boundPortal.ClosePortal();

            OnRetreatedDefeated();
        }
    }
}

