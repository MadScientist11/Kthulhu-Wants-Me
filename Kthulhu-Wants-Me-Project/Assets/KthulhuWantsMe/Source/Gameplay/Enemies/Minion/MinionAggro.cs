using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAggro : MonoBehaviour
    {
        [SerializeField] private MinionAIBrain _minionAIBrain;
        
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
                _minionAIBrain.HasAggro = true;
                _minionAIBrain.IsInAttackRange = PlayerIsInAttackRange();
            }
            else
            {
                _minionAIBrain.HasAggro = false;
            }
        }
        
        private bool PlayerInRange()
        {
            return DistanceToPlayer() < 5f;
        }
        
        private bool PlayerIsInAttackRange() => 
            DistanceToPlayer() < 2f;

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
    }
}