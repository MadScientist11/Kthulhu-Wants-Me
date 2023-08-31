using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MinionFollow : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        public void FollowPlayer()
        {
            MoveTo(_player.transform.position);
        }
        
        private void MoveTo(Vector3 destination) => 
            _navMeshAgent.destination = destination;
    }
}