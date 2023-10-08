using System;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IUpgradeService
    {
        void ApplyUpgrade(UpgradeData upgradeData);
    }
    
    public class UpgradeService : IUpgradeService
    {
        private readonly ThePlayer _player;

        public UpgradeService(ThePlayer player)
        {
            _player = player;
        }
        
        public void ApplyUpgrade(UpgradeData upgradeData)
        {
            IUpgrade upgrade = upgradeData.UpgradeType switch
            {
                UpgradeType.StatUpgrade => new StatUpgrade(upgradeData, _player),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            upgrade.DoUpgrade();
        }
    }
}