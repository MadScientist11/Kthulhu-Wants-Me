using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public interface IPortalFactory
    {
        Portal GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation, EnemyType portalType);
    }
  
    public class PortalFactory : PooledFactory<Portal>, IPortalFactory
    {
        private EnemyType _portalType;
        
        private IObjectResolver _instantiator;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }

        public Portal GetOrCreatePortal(Vector3 spawnPoint, Quaternion rotation, EnemyType portalType)
        {
            _portalType = portalType;
            Portal portal = Get(portal => portal.EnemyType == portalType);
            portal.transform.position = spawnPoint;
            portal.transform.rotation = rotation;
            portal.Show();
            return portal;
        }
        
        protected override Portal Create()
        {
            Portal portalPrefab = _portalType switch
            {
                EnemyType.Tentacle or EnemyType.BleedTentacle or EnemyType.PoisonousTentacle or EnemyType.TentacleSpecial => _dataProvider.PortalConfig.TentaclePortalPrefab,
                _ => _dataProvider.PortalConfig.MinionsPortalPrefab
            };
            Debug.Log(portalPrefab);
            portalPrefab.gameObject.SetActive(false);
            Portal portal = _instantiator.Instantiate(portalPrefab);
            portal.EnemyType = _portalType;
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