using System;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IUpgradeService
    {
        void ApplyUpgrade(UpgradeData upgradeData);
    }
    
    public class UpgradeService : IUpgradeService
    {
        public void ApplyUpgrade(UpgradeData upgradeData)
        {
            IUpgrade upgrade = upgradeData.UpgradeType switch
            {
                UpgradeType.StatUpgrade => new StatUpgrade(upgradeData),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            upgrade.DoUpgrade();
        }
    }
}