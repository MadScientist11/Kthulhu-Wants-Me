using System;
using KthulhuWantsMe.Source.Gameplay.Portal;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IPortalFactory
    {
        PortalEnemySpawner GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation);
    }

    public class PortalFactory : PooledFactory<PortalEnemySpawner>, IPortalFactory
    {
        private IObjectResolver _instantiator;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }

        public PortalEnemySpawner GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation)
        {
            PortalEnemySpawner portal = Get(null);
            portal.transform.position = spawnPoint;
            portal.transform.rotation = rotation;
            portal.Show();
            return portal;
        }

        protected override PortalEnemySpawner Create()
        {
            PortalEnemySpawner portalPrefab = _dataProvider.PortalConfig.PortalPrefab;
            portalPrefab.gameObject.SetActive(false);
            PortalEnemySpawner portal = _instantiator.Instantiate(portalPrefab);
            return portal;
        }

        protected override void Release(PortalEnemySpawner portal)
        {
            base.Release(portal);
            portal.Hide();
        }

        protected override PortalEnemySpawner Get(Func<PortalEnemySpawner, bool> predicate)
        {
            PortalEnemySpawner portal = base.Get(predicate);
            return portal;
        }
    }
}