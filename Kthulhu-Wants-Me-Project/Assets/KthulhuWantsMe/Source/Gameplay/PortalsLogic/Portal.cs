using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public class Portal : MonoBehaviour, IPoolable<Portal>
    {
        public Action<Portal> Release { get; set; }

        public EnemyType PortalType;
        
        [SerializeField] private Transform _initialLocation;
        [SerializeField] private Transform _desiredLocation;

        private TentacleEmergence _spawnedTentacle;

        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Show() =>
            gameObject.SwitchOn();

        public void Hide() =>
            gameObject.SwitchOff();

        public void StartEnemySpawn()
        {
            switch (PortalType)
            {
                case EnemyType.PoisonousTentacle:
                case EnemyType.Tentacle:
                    
                    if (_spawnedTentacle == null)
                        CreateBoundedTentacle();
                    
                    EmergeTentacle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ClosePortal()
        {
            Release?.Invoke(this);
            //_spawnedTentacle.gameObject.SwitchOff();
        }

        private void EmergeTentacle()
        {
            //_spawnedTentacle.gameObject.SwitchOn();
            _spawnedTentacle.Emerge(_initialLocation.transform.position, _desiredLocation.transform.position);
            _spawnedTentacle.GetComponent<TentacleRetreat>().Init(this);
        }

        private void CreateBoundedTentacle()
        {
         
            _spawnedTentacle =
                _gameFactory.CreateEnemy(_initialLocation.position, _initialLocation.rotation, PortalType).GetComponent<TentacleEmergence>();
        }
    }
}