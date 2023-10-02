using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;

namespace KthulhuWantsMe.Source.Gameplay.Upgrades
{
    public class HealthUpgrade : IUpgrade
    {
        public UpgradeInfo UpgradeInfo => new UpgradeInfo()
        {
            Name = "Health Upgrade",
            Description = "Increase health by 10"
        };
        
        private readonly ThePlayer _player;
        private readonly int _value;

        public HealthUpgrade(ThePlayer player, int value)
        {
            _value = value;
            _player = player;
        }


        public void DoUpgrade()
        {
            _player.PlayerStats.Mods[StatType.MaxHealth] = 10;
        }
    }
}