using System;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MinionFollow : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private PlayerFacade _player;
        private Vector3 _randomOffset;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
            _randomOffset = Random.insideUnitCircle.XZtoXYZ() * 2;
        }

        public void FollowPlayer()
        {
            MoveTo(_player.transform.position + _randomOffset);
        }
        
        private void MoveTo(Vector3 destination) => 
            _navMeshAgent.destination = destination;
    }
}