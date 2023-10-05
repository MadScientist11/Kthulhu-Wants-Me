using System;
using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMovement : MonoBehaviour
    {
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

        public void MoveTo(Vector3 destination) => 
            _navMeshAgent.destination = destination;

        public void Disable() => _navMeshAgent.isStopped = true;
        public void Enable() => _navMeshAgent.isStopped = false;
        
    }
}