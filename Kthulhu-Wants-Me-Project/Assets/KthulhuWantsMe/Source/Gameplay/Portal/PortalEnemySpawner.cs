using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities;
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
        private EnemyType _enemyType;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(IGameFactory gameFactory, IPortalSystem portalSystem, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _portalSystem = portalSystem;
            _gameFactory = gameFactory;

        }

        private void OnEnable()
        {
            if (_createdEnemy == null)
            {
                _enemyType = EnemyType.Tentacle;
                _createdEnemy = _gameFactory.CreateEnemy(_tentacleSpawnPoint.position, _tentacleSpawnPoint.rotation,
                    _enemyType);
                _createdEnemy.transform.SetParent(transform);
            }

            if (_enemyType == EnemyType.Tentacle)
            {
                TentacleOnEnable();

            }
            else
            {
                MinionOnEnable();
            }
        }

        private void OnDisable()
        {
            if (_enemyType == EnemyType.Tentacle)
            {
                TentacleOnDisable();

            }
            else
            {
                MinionOnDisable();
            }
        }

        private void TentacleOnEnable()
        {
            _createdEnemy.GetComponent<TentacleEmergence>().Emerge(_tentacleSpawnPoint.position, transform.position);
            _createdEnemy.GetComponent<Health>().RestoreHp();
            _createdEnemy.GetComponent<TentacleRetreat>().OnRetreated += ClosePortal;
        }

        private void TentacleOnDisable()
        {
            _createdEnemy.GetComponent<TentacleRetreat>().OnRetreated -= ClosePortal;
        }


        private void MinionOnEnable()
        {
            _createdEnemy.GetComponent<MinionEmergence>().Emerge(_tentacleSpawnPoint.position, transform.position);
        }

        private void MinionOnDisable()
        {
            
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