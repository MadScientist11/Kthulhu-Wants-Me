using System;
using KthulhuWantsMe.Source.Gameplay.Player.State;

namespace KthulhuWantsMe.Source.Gameplay.UpgradeSystem
{
    public class StatUpgrade : IUpgrade
    {
        private readonly UpgradeData _upgrade;
        private readonly ThePlayer _player;

        public StatUpgrade(UpgradeData upgrade, ThePlayer player)
        {
            _player = player;
            _upgrade = upgrade;
        }

        public void DoUpgrade()
        {
            float newValue = CalculateStat(_player.PlayerStats.MainStats[_upgrade.StatType], _upgrade);
            _player.PlayerStats.ChangeStat(_upgrade.StatType, newValue);
        }

        private float CalculateStat(float baseValue, UpgradeData upgrade)
        {
            return upgrade.UpgradeValueType switch
            {
                UpgradeValueType.Flat => baseValue + upgrade.Value,
                UpgradeValueType.Percent => baseValue + baseValue * upgrade.Value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}