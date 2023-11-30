using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaMovement : MonoBehaviour
    {
        [SerializeField] private CyaeghaAnimator _cyaeghaAnimator;
        [SerializeField] private FollowPlayer _followPlayerBehaviour;

        private IAIService _aiService;

        [Inject]
        public void Construct(IAIService aiService) 
            => _aiService = aiService;
        
        public void Stop()
        {
            _cyaeghaAnimator.StopMove();
        }

        private void Update()
        {
            if (_followPlayerBehaviour.PlayerReached)
            {
                Stop();
            }
        }

        public void FollowPlayer()
        {
            _cyaeghaAnimator.PlayMove();
            
            int pathfindingMethod = DecidePathfinding();
            _followPlayerBehaviour.MoveToPlayer(pathfindingMethod);
        }

        private int DecidePathfinding() => 
            _aiService.EnemiesCount < 10 ? 1 : -1;
        
    }
}