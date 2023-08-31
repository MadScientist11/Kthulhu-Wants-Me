using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        public void SetDestination(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }
    }
}