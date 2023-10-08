using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public class Portal : MonoBehaviour, IPoolable<Portal>, IAbility
    {
        public Action<Portal> Release { get; set; }
        
        public GameObject LastSpawnedEnemy { get; private set; }

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

        public void StartEnemySpawn(EnemyType enemyType)
        {
            GameObject enemy = _gameFactory.CreateEnemy(transform.position, transform.rotation, enemyType);
            LastSpawnedEnemy = enemy;
            
            if (!enemy.TryGetComponent(out EmergeState emergeState))
            {
                ClosePortal();
                Debug.Log("Enemy doesn't have emergence behaviour");
                return;
            }
            
            
        }
        

        public void ClosePortal()
        {
            Release?.Invoke(this);
            //_spawnedTentacle.gameObject.SwitchOff();
        }

    
    }
}