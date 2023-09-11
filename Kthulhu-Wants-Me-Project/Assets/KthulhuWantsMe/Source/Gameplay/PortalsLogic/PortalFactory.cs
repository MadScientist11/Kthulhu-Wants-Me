using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public interface IPortalFactory
    {
        Portal GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation, PortalFactory.PortalType portalType);
    }
    public class PortalFactory : PooledFactory<Portal>, IPortalFactory
    {
        public enum PortalType
        {
            TentaclePortal = 0,
            PoisonTentaclePortal = 1,
        }
        private PortalType _portalType;
        
        private IObjectResolver _instantiator;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }

        public Portal GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation, PortalType portalType)
        {
            _portalType = portalType;
            Portal portal = Get(portal => portal.PortalType == portalType);
            portal.transform.position = spawnPoint;
            portal.transform.rotation = rotation;
            portal.Show();
            return portal;
        }
        
        protected override Portal Create()
        {
            Portal portalPrefab = _portalType switch
            {
                PortalType.TentaclePortal => _dataProvider.PortalConfig.TentaclePortalPrefab,
                PortalType.PoisonTentaclePortal => _dataProvider.PortalConfig.TentaclePortalPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            portalPrefab.gameObject.SetActive(false);
            Portal portal = _instantiator.Instantiate(portalPrefab);
            portal.PortalType = _portalType;
            return portal;
        }

        protected override void Release(Portal portal)
        {
            base.Release(portal);
            portal.Hide();
        }

        protected override Portal Get(Func<Portal, bool> predicate)
        {
            Portal portal = base.Get(predicate);
            return portal;
        }
    }
}