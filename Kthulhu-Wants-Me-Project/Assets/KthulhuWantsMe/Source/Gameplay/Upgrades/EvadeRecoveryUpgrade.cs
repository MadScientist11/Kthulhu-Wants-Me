using KthulhuWantsMe.Source.Gameplay.Player.State;

namespace KthulhuWantsMe.Source.Gameplay.Upgrades
{
    public class EvadeRecoveryUpgrade : IUpgrade
    {
        private ThePlayer _player;
        private float _value;

        public UpgradeInfo UpgradeInfo => new UpgradeInfo()
        {
            Name = "Evade REcovery",
            Description = $"Increase Evade Recovery ({_value})"
        };

        public EvadeRecoveryUpgrade(ThePlayer player, float value)
        {
            _value = value;
            _player = player;
        }
        public void DoUpgrade()
        {
            _player.PlayerStats.EvadeDelay -= _value;
        }
    }
}