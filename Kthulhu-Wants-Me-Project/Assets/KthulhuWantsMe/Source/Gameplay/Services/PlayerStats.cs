using System;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPlayerStats
    {
        Stats Stats { get; }
        void ApplyBuff(IBuff buffItem);
    }


    public class PlayerStats : IPlayerStats
    {
        public Stats Stats { get; }
        
        
        
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly PlayerFacade _player;
        private readonly IGameFactory _gameFactory;

        public PlayerStats(IDataProvider dataProvider, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _playerConfiguration = dataProvider.PlayerConfig;
            Stats = new Stats()
            {
                MaxHealth = _playerConfiguration.MaxHealth,
                BaseDamage = _playerConfiguration.BaseDamage,
                Damage = _playerConfiguration.BaseDamage,
            };
        }

        public void ApplyBuff(IBuff buff)
        {
            switch (buff.BuffTarget)
            {
                case BuffTarget.InstaHealthReplenish:
                    _gameFactory.Player.PlayerHealth.Heal(buff.Value);
                    break;
                case BuffTarget.DamageBuff:
                    Stats.Damage += buff.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}