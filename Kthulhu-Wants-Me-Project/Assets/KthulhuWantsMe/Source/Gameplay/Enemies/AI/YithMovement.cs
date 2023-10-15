using System;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
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

        [MinMaxSlider(3, 12)]
        [SerializeField] private Vector2 _minMaxTargetOffset = new(4, 8);
        
        [Tooltip("Apply offset to target, if distance to target is greater than the threshold")]
        [Range(3, 8)]
        [SerializeField] private float _offsetMovementThreshold = 5f;

        private Vector3 _playerTarget;

        private float _resetOffsetTimer;
        private const float ResetOffsetTime = 2f;

        private IInterceptionCompliant _interceptTarget;

        [Range(-1f, 1f)] 
        [SerializeField] private float _movementPredictionThreshold = 0;
        [Range(0.25f, 2f)] 
        [SerializeField] private float _movementPredictionTime = 1;

        private int _pathfindingMethod;

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
            _pathfindingMethod = Random.Range(0, 2);
        }

        private void Start()
        {
            if (_player.TryGetComponent(out IInterceptionCompliant interceptionCompliant))
            {
                _interceptTarget = interceptionCompliant;
            }
        }

        public void MoveToPlayer()
        {
            _playerTarget = _player.transform.position;

            if (_pathfindingMethod == 0)
                RandomOffsetPathfinding();
            else
                InterceptionAverageVelocityBasedPathfinding();


            _movementMotor.MoveTo(_playerTarget);
        }

        private void RandomOffsetPathfinding()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > _offsetMovementThreshold)
            {
                if (Vector3.Distance(_player.transform.position, _lastNearPoint) > _minMaxTargetOffset.y)
                {
                    _lastNearPoint = NearPlayerRandomPoint();
                    _resetOffsetTimer = ResetOffsetTime;
                }

                _resetOffsetTimer -= Time.deltaTime;

                if (_resetOffsetTimer > 0)
                    _playerTarget = _lastNearPoint;
            }
        }

        private void InterceptionAverageVelocityBasedPathfinding()
        {
            if (_interceptTarget == null)
            {
                RandomOffsetPathfinding();
                Debug.LogWarning("Interception target is null.");
                return;
            }

            float timeToPlayer = Vector3.Distance(_player.transform.position, transform.position) / _movementMotor.Agent.speed;

            if (timeToPlayer > _movementPredictionTime)
            {
                timeToPlayer = _movementPredictionTime;
            }

            _playerTarget = _player.transform.position + _interceptTarget.AverageVelocity * timeToPlayer;

            Vector3 directionToTarget = (_playerTarget - transform.position).normalized;
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(directionToPlayer, directionToTarget);

            if (dot < _movementPredictionThreshold)
            {
                _playerTarget = _player.transform.position;
            }
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
                float targetOffset = Random.Range(_minMaxTargetOffset.x, _minMaxTargetOffset.y);
                Vector3 randomPoint = _player.transform.position + Random.insideUnitSphere * targetOffset;
                bool sampleSuccess =
                    NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, targetOffset, NavMesh.AllAreas);

                if (sampleSuccess)
                {
                    _movementMotor.Agent.CalculatePath(hit.position, _navMeshPath);

                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete &&
                        !NavMesh.Raycast(_player.transform.position, hit.position, out NavMeshHit _, NavMesh.AllAreas))
                    {
                        result = hit.position;
                        pointFound = true;
                    }
                }
            }

            return result;
        }
    }
}