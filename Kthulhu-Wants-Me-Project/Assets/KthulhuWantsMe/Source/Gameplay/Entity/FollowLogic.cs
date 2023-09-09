using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
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
        private float _maxFollowDistance;
        private float _reachDistance;

        private void OnValidate() => 
            _movementMotor = GetComponent<NavMeshMovement>();

        private void Update()
        {
            _movementMotor.MoveTo(_followTarget.position);
        }

        public void Init(Transform followTarget, float maxFollowDistance, float reachDistance, bool followOnStart = false)
        {
            _reachDistance = reachDistance;
            _maxFollowDistance = maxFollowDistance;
            SetFollowTarget(followTarget);
            // SetStoppingDistance
            
            if(followOnStart)
                Follow();
            else
                StopFollowing();
        }

        public void SetFollowTarget(Transform target) => 
            _followTarget = target;
        
        public void Follow() => 
            _movementMotor.Enable();
        
        public void StopFollowing() => 
            _movementMotor.Disable();

            
        public bool CanFollow() => Vector3.Distance(transform.position, _followTarget.position) <= _maxFollowDistance;
    }
}