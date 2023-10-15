using System;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    public class YithMovement : MonoBehaviour
    {
        [SerializeField] private MovementMotor _movementMotor;
        
        private NavMeshPath _navMeshPath;
        private Vector3 _lastNearPoint;

        private const float PlayerTargetOffset = 8f;
        private const float OffsetMovementThreshold = 5f;

        private Vector3 _playerTarget;

        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Awake()
        {
            _navMeshPath = new();
            _movementMotor.Agent.speed = Random.Range(3f, 4f);
        }

        public void MoveToPlayer()
        {
            _playerTarget = _player.transform.position;

            if (Vector3.Distance(_player.transform.position, transform.position) > OffsetMovementThreshold)
            {
                if (Vector3.Distance(_player.transform.position, _lastNearPoint) > PlayerTargetOffset)
                {
                    _lastNearPoint = NearPlayerRandomPoint();
                }

                _playerTarget = _lastNearPoint;
            }

            _movementMotor.MoveTo(_playerTarget);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_playerTarget, .5f);
        }

        private Vector3 NearPlayerRandomPoint()
        {
            bool pointFound = false;

            Vector3 result = Vector3.zero;

            while (!pointFound)
            {
                Vector3 randomPoint = _player.transform.position + Random.insideUnitSphere * PlayerTargetOffset;
                bool sampleSuccess = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, PlayerTargetOffset, NavMesh.AllAreas);

                if (sampleSuccess)
                {
                    _movementMotor.Agent.CalculatePath(hit.position, _navMeshPath);

                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete &&
                        !NavMesh.Raycast(_player.transform.position, randomPoint, out NavMeshHit _, NavMesh.AllAreas))
                    {
                        result = randomPoint;
                        pointFound = true;
                    }
                }
            }

            return result;
        }
    }
}