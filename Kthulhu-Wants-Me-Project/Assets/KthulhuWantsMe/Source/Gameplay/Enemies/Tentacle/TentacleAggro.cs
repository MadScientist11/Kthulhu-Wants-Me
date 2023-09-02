using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAggro : MonoBehaviour
    {
        public bool HasAggro { get; private set; }
        
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }
        
        private void Update()
        {
            if (PlayerInRange())
            {
                _tentacleAnimator.PlayAggroIdle();
                HasAggro = true;
            }
        }
        
        private bool PlayerInRange()
        {
            return DistanceToPlayer() < 5f;
        }
        
        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
    }
}
