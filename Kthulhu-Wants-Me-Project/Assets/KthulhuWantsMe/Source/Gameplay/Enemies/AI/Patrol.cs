using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    [RequireComponent(typeof(MovementMotor))]
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private MovementMotor _movementMotor;
        
        private NavMeshPath _navMeshPath;
        private int _maxIterations;
        private Vector2 _minMaxTargetOffset = new(5,20);
        
        private Vector3 _target;
        private bool _patrol;
        private void Awake()
        {
            _navMeshPath = new();
            _movementMotor.Agent.speed = Random.Range(3f, 4f);
        }

        public void PatrolArea()
        {
            if (!_patrol)
            {
                _patrol = true;
                _target = transform.position;
            }
            
            if (Vector3.Distance(_target, transform.position) < 3)
            {
                _target = NearRandomPoint();
                
            }
            _movementMotor.MoveTo(_target);
        }
        
        public void CancelPatrol()
        {
            _patrol = false;
        }
        
        private Vector3 NearRandomPoint()
        {
            if (_maxIterations > 500)
                return Vector3.zero;

            bool pointFound = false;

            Vector3 result = Vector3.zero;

            while (!pointFound)
            {
                float targetOffset = Random.Range(_minMaxTargetOffset.x, _minMaxTargetOffset.y);
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * targetOffset;
                bool sampleSuccess =
                    NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, targetOffset, NavMesh.AllAreas);

                _maxIterations++;
                if (sampleSuccess)
                {
                    _movementMotor.Agent.CalculatePath(hit.position, _navMeshPath);

                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete &&
                        !NavMesh.Raycast(transform.position, hit.position, out NavMeshHit _, NavMesh.AllAreas))
                    {
                        result = hit.position;
                        pointFound = true;
                        _maxIterations = 0;
                    }
                }

                if (_maxIterations > 500)
                    return Vector3.zero;
            }

            return result;
        }
    }
}
