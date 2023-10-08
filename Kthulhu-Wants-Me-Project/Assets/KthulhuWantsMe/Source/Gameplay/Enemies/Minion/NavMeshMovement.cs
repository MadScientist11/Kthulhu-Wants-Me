using System;
using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMovement : MonoBehaviour
    {
        public bool Enabled => _navMeshAgent.enabled;
        
        public float MoveSpeed
        {
            get => _navMeshAgent.speed;
            set => _navMeshAgent.speed = value;
        }
        
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private Transform _followTarget;

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 destination)
        {
            if(_navMeshAgent.enabled)
                _navMeshAgent.destination = destination;
        }

        public void HaltMovement()
        {
            if(_navMeshAgent.enabled)
                _navMeshAgent.isStopped = true;
        }

        public void ResumeMovement()
        {
            if(_navMeshAgent.enabled)
                _navMeshAgent.isStopped = false;
        }

        public void Disable() => _navMeshAgent.SwitchOff();

        public void Enable() => _navMeshAgent.SwitchOn();
        

    }
}