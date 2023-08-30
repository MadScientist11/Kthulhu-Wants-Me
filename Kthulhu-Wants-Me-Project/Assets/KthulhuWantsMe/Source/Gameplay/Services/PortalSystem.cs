using System.Collections;
using System.Collections.Generic;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Portal;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = Freya.Random;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPortalSystem
    {
        void Init();
        void SpawnPortals();
        void Release(PortalEnemySpawner portal);
    }

    public class PortalSystem : IPortalSystem, ITickable
    {
        private bool _isInitialized;

        private List<PortalSpawnZone> _portalSpawnZone;

        private Location _location;
        private IPortalFactory _portalFactory;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(IPortalFactory portalFactory, ICoroutineRunner coroutineRunner, Location location)
        {
            _coroutineRunner = coroutineRunner;
            _portalFactory = portalFactory;
            _location = location;
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
            PortalZone spawnZone = _location.PortalSpawnZones[Random.Range(0, _location.PortalSpawnZones.Count)];
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 randomPoint = new Vector3(randomX, 0, randomZ);
            Vector3 orientedRandomPoint =
                spawnZone.LocalToWrold * new Vector4(randomPoint.x, randomPoint.y, randomPoint.z, 1);
            _portalFactory.GetOrCreatePortal(orientedRandomPoint, spawnZone.Rotation);
        }

        public void Release(PortalEnemySpawner portal)
        {
            portal.Release?.Invoke(portal);
        }

        private IEnumerator Reappear()
        {
            while (true)
            {
                SpawnPortal();
                yield return new WaitForSeconds(5f);
            }
        }
    }
}