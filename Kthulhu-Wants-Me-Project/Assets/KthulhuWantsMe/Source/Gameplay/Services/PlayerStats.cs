using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPlayerStats : IDamageProvider
    {
        void AddDamageProvider(IDamageProvider damageProvider);
        void RemoveDamageProvider(IDamageProvider damageProvider);
    }

    public class PlayerBaseDamageProvider : IDamageProvider
    {
        private readonly PlayerConfiguration _playerConfiguration;

        public PlayerBaseDamageProvider(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public float ProvideDamage()
        {
            return _playerConfiguration.BaseDamage;
        }
    }

    public class PlayerStats : IPlayerStats, IDisposable
    {
        private readonly HashSet<IDamageProvider> _damageProviders = new();
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly IInventorySystem _inventorySystem;

        public PlayerStats(IDataProvider dataProvider, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _playerConfiguration = dataProvider.PlayerConfig;
            AddDamageProvider(new PlayerBaseDamageProvider(_playerConfiguration));
            _inventorySystem.OnItemAdded += AddItemStatsIfAny;
            _inventorySystem.OnItemRemoved += RemoveItemStatsIfAny;
            _inventorySystem.OnItemSwitched += ItemSwitched;
        }

        public void Dispose()
        {
            _inventorySystem.OnItemAdded -= AddItemStatsIfAny;
            _inventorySystem.OnItemRemoved -= RemoveItemStatsIfAny;
            _inventorySystem.OnItemSwitched -= ItemSwitched;
        }

        public void AddDamageProvider(IDamageProvider damageProvider)
        {
            _damageProviders.Add(damageProvider);
            Debug.Log(ProvideDamage());
        }

        public void RemoveDamageProvider(IDamageProvider damageProvider)
        {
            _damageProviders.Remove(damageProvider);
        }

        public float ProvideDamage() =>
            _damageProviders.Sum(provider => provider.ProvideDamage());

        private void AddItemStatsIfAny(IPickable item)
        {
            if (item.IsWeapon(out IDamageProvider damageProvider))
            {
                AddDamageProvider(damageProvider);
            }
        }

        private void RemoveItemStatsIfAny(IPickable item)
        {
            if (item.IsWeapon(out IDamageProvider damageProvider))
            {
                RemoveDamageProvider(damageProvider);
            }
        }

        private void ItemSwitched(IPickable item, IPickable previousItem)
        {
            RemoveItemStatsIfAny(previousItem);
            AddItemStatsIfAny(previousItem);
        }
    }
}