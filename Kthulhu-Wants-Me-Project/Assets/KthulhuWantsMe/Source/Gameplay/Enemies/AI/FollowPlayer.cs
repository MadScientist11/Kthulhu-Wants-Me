using System;
using Cysharp.Threading.Tasks;
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
    public class FollowPlayer : MonoBehaviour
    {
        public bool PlayerReached => DistanceToPlayer <= _reachDistance;

        public float DistanceToPlayer => Vector3.Distance(transform.position, _player.transform.position);

        [SerializeField] private MovementMotor _movementMotor;

        private NavMeshPath _navMeshPath;
        private Vector3 _lastNearPoint;

        [MinMaxSlider(3, 12)] [SerializeField] private Vector2 _minMaxTargetOffset = new(4, 8);

        [Tooltip("Apply offset to target, if distance to target is greater than the threshold")]
        [Range(3, 8)]
        [SerializeField]
        private float _offsetMovementThreshold = 5f;

        private Vector3 _playerTarget;

        private float _resetOffsetTimer;
        private const float ResetOffsetTime = 2f;
        private const int TryFindNearPlayerPointMaxIterations = 500;
        private int _maxIterations;

        private IInterceptionCompliant _interceptTarget;

        [Range(-1f, 1f)] [SerializeField] private float _movementPredictionThreshold = 0;
        [Range(0.25f, 2f)] [SerializeField] private float _movementPredictionTime = 1;
        [Range(0f, 5f)] [SerializeField] private float _reachDistance = 3f;

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
            _movementMotor.Agent.stoppingDistance = _reachDistance;
            _pathfindingMethod = Random.Range(0, 2);
        }

        private void Start()
        {
            if (_player.TryGetComponent(out IInterceptionCompliant interceptionCompliant))
            {
                _interceptTarget = interceptionCompliant;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_playerTarget, .5f);
        }

        public void MoveToPlayer(int pathfindingMethod = -1)
        {
            _playerTarget = _player.transform.position;

            if (PlayerReached)
            {
                FaceTarget(_playerTarget);
                _movementMotor.MoveTo(_playerTarget);
                return;
            }

            if (DistanceToPlayer >= _reachDistance)
            {
                UpdateTarget(pathfindingMethod);
            }

            _movementMotor.MoveTo(_playerTarget);
        }

        private void FaceTarget(Vector3 destination)
        {
            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3);
        }

        private void UpdateTarget(int selectedPathfinding)
        {
            if (selectedPathfinding != -1)
            {
                SelectPathfinding(selectedPathfinding);
                return;
            }
            SelectPathfinding(_pathfindingMethod);
        }

        private void SelectPathfinding(int method)
        {
            if (method == 0)
                RandomOffsetPathfinding();
            else
                InterceptionAverageVelocityBasedPathfinding();
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

            float timeToPlayer = Vector3.Distance(_player.transform.position, transform.position) /
                                 _movementMotor.Agent.speed;

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

        private Vector3 NearPlayerRandomPoint()
        {
            if (_maxIterations > 500)
                return Vector3.zero;

            bool pointFound = false;

            Vector3 result = Vector3.zero;

            while (!pointFound)
            {
                float targetOffset = Random.Range(_minMaxTargetOffset.x, _minMaxTargetOffset.y);
                Vector3 randomPoint = _player.transform.position + Random.insideUnitSphere * targetOffset;
                bool sampleSuccess =
                    NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, targetOffset, NavMesh.AllAreas);

                _maxIterations++;
                if (sampleSuccess)
                {
                    _movementMotor.Agent.CalculatePath(hit.position, _navMeshPath);

                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete &&
                        !NavMesh.Raycast(_player.transform.position, hit.position, out NavMeshHit _, NavMesh.AllAreas))
                    {
                        result = hit.position;
                        pointFound = true;
                        _maxIterations = 0;
                        Debug.Log($"Switch pathfinding {_maxIterations}");
                    }
                }

                if (_maxIterations > 500)
                    return Vector3.zero;
            }

            return result;
        }
    }
}