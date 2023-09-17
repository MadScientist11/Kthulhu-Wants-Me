using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPlayerStats
    {
        Stats Stats { get; }
        float GetOverallDamage();
    }


    public class PlayerStats : IPlayerStats
    {
        public Stats Stats { get; }
        
        
        
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly PlayerFacade _player;
        private readonly IGameFactory _gameFactory;
        private readonly IInventorySystem _inventorySystem;

        public PlayerStats(IDataProvider dataProvider, IGameFactory gameFactory, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _gameFactory = gameFactory;
            _playerConfiguration = dataProvider.PlayerConfig;
            Stats = new Stats()
            {
                MaxHealth = _playerConfiguration.MaxHealth,
                BaseDamage = _playerConfiguration.BaseDamage,
                Damage = _playerConfiguration.BaseDamage,
            };
        }

        public float GetOverallDamage()
        {
            float weaponDamage = (_inventorySystem.CurrentItem is WeaponItem weaponItem)
                ? weaponItem.WeaponData.BaseDamage
                : 0;
            return Stats.BaseDamage + weaponDamage;
        }
    }
}