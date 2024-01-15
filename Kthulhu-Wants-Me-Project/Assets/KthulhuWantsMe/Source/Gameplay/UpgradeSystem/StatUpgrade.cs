using System;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Stats;
using UnityEngine;

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

            if (_upgrade.StatType == StatType.MaxHealth)
            {
                _player.Heal(Mathf.Floor(_player.PlayerStats.MainStats[StatType.MaxHealth] * 0.1f));
            }

            //if (_upgrade.StatType == StatType.MaxHealth)
            //{
            //    _player.RestoreHp();
            //}
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