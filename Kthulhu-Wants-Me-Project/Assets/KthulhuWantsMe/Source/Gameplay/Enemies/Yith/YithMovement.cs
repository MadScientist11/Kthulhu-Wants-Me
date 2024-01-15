using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithMovement : MonoBehaviour
    {
        [SerializeField] private FollowPlayer _followPlayerBehaviour;
        [SerializeField] private Patrol _patrolBehaviour;
        
        [SerializeField] private YithAnimator _yithAnimator;
        [SerializeField] private MovementMotor _movementMotor;
        
        private IAIService _aiService;

        [Inject]
        public void Construct(IAIService aiService) 
            => _aiService = aiService;


        public void Stop()
        {
            _yithAnimator.StopMove();
        }

        private void Update()
        {
            if (_followPlayerBehaviour.PlayerReached)
            {
                Stop();
            }
        }

        public void PatrolArea()
        {
            _yithAnimator.PlayMove();
            _patrolBehaviour.PatrolArea();
        }

        public void FollowPlayer()
        {
            _yithAnimator.PlayMove();
            _patrolBehaviour.CancelPatrol();
            
            int pathfindingMethod = DecidePathfinding();
            _followPlayerBehaviour.MoveToPlayer(pathfindingMethod);
        }

        private int DecidePathfinding() => 
            _aiService.EnemiesCount < 10 ? 1 : -1;
    }
}