using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleRetreat : MonoBehaviour
    {
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleEmergence _tentacleEmergence;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;
        [SerializeField] private TentacleHealth _tentacleHealth;
        
        private Portal _boundPortal;

        public void Init(Portal boundPortal)
        {
            _boundPortal = boundPortal;
        }
        
        private void Start()
        {
            _tentacleHealth.Died += Retreat;
        }

        private void OnDestroy()
        {
            _tentacleHealth.Died -= Retreat;
        }

        private void Retreat()
        {
            _tentacleAnimator.PlayRetreat();
            _tentacleAIBrain.BlockProcessing = true;
            StartCoroutine(RetreatToPortal(_tentacleEmergence.InitialPoint));
        }

        private IEnumerator RetreatToPortal(Vector3 to)
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = to;

            float duration = 4f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _boundPortal.ClosePortal();
        }

    }
}