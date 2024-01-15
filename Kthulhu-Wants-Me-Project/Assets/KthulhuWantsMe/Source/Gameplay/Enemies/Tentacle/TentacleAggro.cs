using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAggro : MonoBehaviour
    {
        public bool HasAggro { get; private set; }
        public bool IsPlayerInFront { get; private set; }
        
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleSpellCastingAbility _tentacleSpellCastingAbility;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private float _speed = 90;
        
        private PlayerFacade _player;
        private TentacleConfiguration _tentacleConfiguration;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _tentacleConfiguration = (TentacleConfiguration)_enemyStatsContainer.Config;
        }

        private void Update()
        {
            if (PlayerInRange())
            {
                HasAggro = true;
                _tentacleAnimator.PlayAggroIdle();
              
                Vector3 directionToPlayer = _player.transform.position - transform.position;
                directionToPlayer.Normalize();
                directionToPlayer.y = 0f;


                if (directionToPlayer != Vector3.zero && !_tentacleSpellCastingAbility.CastingSpell && !_tentacleAIBrain.SpecialTentacle)
                {
                    float step = _tentacleConfiguration.RotationSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToPlayer), step);
                }
   
                if (Vector3.Dot(transform.forward, directionToPlayer) > 0.7f)
                {
                    IsPlayerInFront = true;
                }
                else
                {
                    IsPlayerInFront = false;
                }
            }
            else
            {
                HasAggro = false;
                IsPlayerInFront = false;
            }
        }
        
        private bool PlayerInRange()
        {
            return DistanceToPlayer() < _tentacleConfiguration.AggroRange;
        }
        
        public float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
    }
}
