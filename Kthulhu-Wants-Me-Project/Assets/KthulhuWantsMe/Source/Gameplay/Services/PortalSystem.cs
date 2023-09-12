using System.Collections;
using System.Collections.Generic;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;
using Random = Freya.Random;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPortalSystem
    {
        void Init();
        void SpawnPortals();
    }

    public class PortalSystem : IPortalSystem, ITickable
    {
        private bool _isInitialized;

        private List<PortalZone> _portalSpawnZones;

        private IPortalFactory _portalFactory;
        private ICoroutineRunner _coroutineRunner;

        private Collider[] _obstacles = new Collider[1];

        [Inject]
        public void Construct(IPortalFactory portalFactory, ICoroutineRunner coroutineRunner, Location location)
        {
            _coroutineRunner = coroutineRunner;
            _portalFactory = portalFactory;
            _portalSpawnZones = location.PortalSpawnZones;
        }

        public void Init()
        {
            _isInitialized = true;
            _coroutineRunner.StartRoutine(Reappear());
        }

        public void Tick()
        {
            if (!_isInitialized)
                return;

          
        }

        public void SpawnPortals()
        {
        }

        private void SpawnPortal()
        {
            int spawnAttempt = 0;
            while (spawnAttempt < 5)
            {
                PortalZone spawnZone = _portalSpawnZones[Random.Range(0, _portalSpawnZones.Count)];
                Vector3 orientedRandomPoint = GetRandomPointInZone(spawnZone);

                if (IsValidPortalSpawnPoint(orientedRandomPoint))
                {
                    Portal portal = _portalFactory.GetOrCreatePortal(orientedRandomPoint, spawnZone.Rotation,
                        EnemyType.Tentacle);
                    portal.StartEnemySpawn();
                    break;
                }

                spawnAttempt++;
            }
        }

        private bool IsValidPortalSpawnPoint(Vector3 point)
        {
            int obstaclesMask =
                LayerMask.GetMask(GameConstants.Layers.PortalSpawnObstacle, GameConstants.Layers.Player);
            int obstaclesCount = Physics.OverlapSphereNonAlloc(point, 1f, _obstacles,
                obstaclesMask);
            return obstaclesCount == 0;
        }


        private IEnumerator Reappear()
        {
            while (true)
            {
                SpawnPortal();
                yield return Utilities.WaitForSeconds.Wait(20f);
            }
        }

        private Vector3 GetRandomPointInZone(PortalZone spawnZone)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 randomPoint = new Vector3(randomX, 0, randomZ);
            Vector3 orientedRandomPoint =
                spawnZone.LocalToWrold * new Vector4(randomPoint.x, randomPoint.y, randomPoint.z, 1);
            return orientedRandomPoint;
        }
    }
}