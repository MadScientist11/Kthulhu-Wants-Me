using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.GameplayTest
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;

        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start() =>
            SpawnEnemy();


        [Button()]
        private void SpawnEnemy() => 
            _gameFactory.CreatePortalWithEnemy(transform.position, transform.rotation, _enemyType);
    }
}