using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Infrastructure.Services;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public class Portal : MonoBehaviour, IPoolable<Portal>
    {
        public Action<Portal> Release { get; set; }

        public PortalFactory.PortalType PortalType;
        
        [ShowIf("TentaclePortal")]
        public bool SpawnMinions = true;
        
        [SerializeField] private Transform _initialLocation;
        [SerializeField] private Transform _desiredLocation;

        private bool TentaclePortal => PortalType == PortalFactory.PortalType.TentaclePortal;
        
        private GameObject _spawnedTentacle;

        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void OnEnable()
        {
            SpawnEnemy();
        }

        private void OnDisable()
        {
            if (PortalType == PortalFactory.PortalType.TentaclePortal && _spawnedTentacle != null) 
            {
                _spawnedTentacle.GetComponent<TentacleRetreat>().OnRetreated -= ClosePortal;
            }
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        private void SpawnEnemy()
        {
            switch (PortalType)
            {
                case PortalFactory.PortalType.TentaclePortal:
                    
                    if (_spawnedTentacle == null)
                        CreateBoundedTentacle();
                    
                    TentacleEmergence tentacleEmergence = _spawnedTentacle.GetComponent<TentacleEmergence>();
                    tentacleEmergence.Emerge(_initialLocation.transform.position, _desiredLocation.transform.position);
                    _spawnedTentacle.GetComponent<TentacleRetreat>().OnRetreated += ClosePortal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateBoundedTentacle()
        {
            EnemyType enemyType = PortalType switch
            {
                PortalFactory.PortalType.TentaclePortal => EnemyType.Tentacle,
                _ => throw new ArgumentOutOfRangeException()
            };
            _spawnedTentacle =
                _gameFactory.CreateEnemy(_initialLocation.position, _initialLocation.rotation, enemyType);
        }


        private void ClosePortal()
        {
            Release?.Invoke(this);
        }
    }
}