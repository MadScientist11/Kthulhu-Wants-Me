using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAggro : MonoBehaviour
    {
        public bool HasAggro { get; }
        public bool IsPlayerInAttackRange { get; }
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Init()
        {
            
        }
    }
}