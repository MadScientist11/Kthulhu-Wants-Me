using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementMotor : MonoBehaviour
    {
        public NavMeshAgent Agent => _navMeshAgent;

        [SerializeField] private NavMeshAgent _navMeshAgent;

        private float _defaultAngularSpeed;

        private void Start()
        {
            _defaultAngularSpeed = _navMeshAgent.angularSpeed;
        }

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 destination)
        {
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.SetDestination(destination);
            }
        }

        public void AddVelocity(Vector3 velocity, float resetAfter, Action after = null)
        {
            StartCoroutine(DoVelocityChange(velocity, resetAfter, after));
        }

        private IEnumerator DoVelocityChange(Vector3 velocity, float resetAfter, Action action)
        {
            _navMeshAgent.velocity = velocity;
            _navMeshAgent.angularSpeed = 0;
            
            yield return new WaitForSeconds(resetAfter);
            
            _navMeshAgent.angularSpeed = _defaultAngularSpeed;
            _navMeshAgent.velocity = Vector3.zero;
            
            action?.Invoke();
        }

        public void HaltMovement()
        {
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = true;
        }

        public void ResumeMovement()
        {
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = false;
        }

        public void Disable() => _navMeshAgent.SwitchOff();

        public void Enable() => _navMeshAgent.SwitchOn();
    }
}