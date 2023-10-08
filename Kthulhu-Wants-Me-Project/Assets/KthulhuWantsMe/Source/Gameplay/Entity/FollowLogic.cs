using System;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public interface IInterceptionCompliant
    {
        Vector3 AverageVelocity { get; }
    }
    [RequireComponent(typeof(NavMeshMovement))]
    public class FollowLogic : MonoBehaviour
    {
        public bool TargetReached => DistanceToTarget <= _reachDistance;

        public float DistanceToTarget => Vector3.Distance(transform.position, _followTarget.position);

        public float FollowSpeed
        {
            get => _movementMotor.MoveSpeed;
            set => _movementMotor.MoveSpeed = value;
        }
        
        [SerializeField] private NavMeshMovement _movementMotor;
        
        private Transform _followTarget;
        private IInterceptionCompliant _interceptTarget;
        private float _maxFollowDistance;
        private float _reachDistance;
        
        [Range(-1f, 1f)]
        public float MovementPredictionThreshold = 0;
        [Range(0.25f, 2f)]
        public float MovementPredictionTime = 1;

        private void OnValidate() => 
            _movementMotor = GetComponent<NavMeshMovement>();

        private void Update()
        {
            if(!_movementMotor.Enabled)
                return;
            
            if (_interceptTarget != null)
            {

                float timeToPlayer = Vector3.Distance(_followTarget.position, transform.position) /
                                     FollowSpeed;

                if (timeToPlayer > MovementPredictionTime)
                {
                    timeToPlayer = MovementPredictionTime;
                }

                Vector3 targetPosition = _followTarget.position + _interceptTarget.AverageVelocity * timeToPlayer;

                Vector3 directionToTarget = (targetPosition - transform.position).normalized;
                Vector3 directionToPlayer = (_followTarget.position - transform.position).normalized;

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);

                if (dot < MovementPredictionThreshold)
                {
                    targetPosition = _followTarget.position;
                }
                _movementMotor.MoveTo(targetPosition);
            }
        }

        private void OnDrawGizmos()
        {
            float timeToPlayer = Vector3.Distance(_followTarget.position, transform.position) /
                                 FollowSpeed;

            if (timeToPlayer > MovementPredictionTime)
            {
                timeToPlayer = MovementPredictionTime;
            }

            Vector3 targetPosition = _followTarget.position + _interceptTarget.AverageVelocity * timeToPlayer;

            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 directionToPlayer = (_followTarget.position - transform.position).normalized;

            float dot = Vector3.Dot(directionToPlayer, directionToTarget);

            if (dot < MovementPredictionThreshold)
            {
                targetPosition = _followTarget.position;
            }
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPosition, .3f);
        }


        public void Init(Transform target, float maxFollowDistance, float reachDistance, bool followOnStart = false)
        {
            _reachDistance = reachDistance;
            _maxFollowDistance = maxFollowDistance;
            SetFollowTarget(target);
            // SetStoppingDistance
            
            if(followOnStart)
                Follow();
            else
                StopFollowing();
        }

        public void SetFollowTarget(Transform target)
        {
            _followTarget = target;
            if(_followTarget.TryGetComponent(out IInterceptionCompliant interceptionCompliant))
            {
                _interceptTarget = interceptionCompliant;
            }
        }

        public void Follow() => 
            _movementMotor.ResumeMovement();
        
        public void StopFollowing() => 
            _movementMotor.HaltMovement();

        public void DisableMotor() =>
            _movementMotor.Disable();
        
        public void EnableMotor() =>
            _movementMotor.Enable();

            
        public bool CanFollow() => Vector3.Distance(transform.position, _followTarget.position) <= _maxFollowDistance;
    }
}