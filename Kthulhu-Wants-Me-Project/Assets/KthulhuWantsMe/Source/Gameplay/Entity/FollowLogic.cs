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
            if (_interceptTarget != null)
            {
                Debug.Log(_interceptTarget.AverageVelocity);
                _movementMotor.MoveTo(_followTarget.position + _interceptTarget.AverageVelocity * MovementPredictionTime);
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_followTarget.position + _interceptTarget.AverageVelocity * MovementPredictionTime, .3f);
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
            _movementMotor.Enable();
        
        public void StopFollowing() => 
            _movementMotor.Disable();

            
        public bool CanFollow() => Vector3.Distance(transform.position, _followTarget.position) <= _maxFollowDistance;
    }
}