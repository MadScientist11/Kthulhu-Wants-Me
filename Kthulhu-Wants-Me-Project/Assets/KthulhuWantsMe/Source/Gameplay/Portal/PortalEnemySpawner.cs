using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Portal
{
    public class PortalEnemySpawner : MonoBehaviour, IPoolable<PortalEnemySpawner>
    {
        public Action<PortalEnemySpawner> Release { get; set; }
        [SerializeField] private Transform _tentacleSpawnPoint;
        
        private GameObject _createdEnemy;

        private IGameFactory _gameFactory;
        private IPortalSystem _portalSystem;

        [Inject]
        public void Construct(IGameFactory gameFactory, IPortalSystem portalSystem)
        {
            _portalSystem = portalSystem;
            _gameFactory = gameFactory;

        }

        private void OnEnable()
        {
            if (_createdEnemy == null)
            {
                _createdEnemy = _gameFactory.CreateEnemy(_tentacleSpawnPoint.position, _tentacleSpawnPoint.rotation,
                    EnemyType.Tentacle);
                _createdEnemy.transform.SetParent(transform);
            }
           
            _createdEnemy.GetComponent<TentacleEmergence>().Emerge(_tentacleSpawnPoint.position, transform.position);
            _createdEnemy.GetComponent<EnemyHealth>().RestoreHealth();
            _createdEnemy.GetComponent<TentacleRetreat>().OnRetreated += ClosePortal;
        }

        private void OnDisable()
        {
            _createdEnemy.GetComponent<TentacleRetreat>().OnRetreated -= ClosePortal;
        }

        private void ClosePortal()
        {
            _portalSystem.Release(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}