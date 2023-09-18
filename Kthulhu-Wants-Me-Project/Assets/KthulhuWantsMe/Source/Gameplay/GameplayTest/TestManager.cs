using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Infrastructure.Services;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.GameplayTest
{
    public class TestManager : MonoBehaviour
    {
        public Transform EnemySpawnPoint;
        public bool AutoSpawnPlayer;

        private IGameFactory _gameFactory;
        private IPortalFactory _portalFactory;
        private Location _location;
        private IRuntimeData _runtimeData;

        [Inject]
        public void Construct(IPortalFactory portalFactory, IGameFactory gameFactory, IRuntimeData runtimeData, Location location)
        {
            _runtimeData = runtimeData;
            _portalFactory = portalFactory;
            _location = location;
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            if (AutoSpawnPlayer)
                SpawnPlayer();
        }

        [Button()]
        private void SpawnPlayer()
        {
            if (_gameFactory.Player == null)
                _gameFactory.CreatePlayer(_location.PlayerSpawnPosition, _location.PlayerSpawnRotation);
        }

        [Button()]
        private void SpawnTentacle_FromPortal_NoMinions()
        {
           //Portal portal = _portalFactory.GetOrCreatePortal(EnemySpawnPoint.position, EnemySpawnPoint.rotation,
           //    PortalFactory.PortalType.TentaclePortal);
           //_runtimeData.TentacleSettings.AllowSpellCasting = false;
        }

        [Button()]
        private void SpawnTentacle_NoPortal_NoMinions()
        {
            GameObject tentacle = _gameFactory.CreateEnemy(EnemySpawnPoint.position, EnemySpawnPoint.rotation, EnemyType.Tentacle);
            //tentacle.GetComponent<TentacleEmergence>().Emerge(EnemySpawnPoint.position, EnemySpawnPoint.position);
        }
        
        [Button()]
        private void SpawnMinionsSpell()
        {
            MinionsSpawnSpell spell = _gameFactory.CreateMinionsSpawnSpell(EnemySpawnPoint.position, EnemySpawnPoint.rotation);
            spell.Activate();
        }
    }
}