using System;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private Vector3 _currentDestination;
        
        private PlayerFacade _player;

        [Inject]
        private void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Update()
        {
            _navMeshAgent.destination = _player.transform.position;
        }

        public void SetDestination(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }

        private bool IsDestinationReached()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}