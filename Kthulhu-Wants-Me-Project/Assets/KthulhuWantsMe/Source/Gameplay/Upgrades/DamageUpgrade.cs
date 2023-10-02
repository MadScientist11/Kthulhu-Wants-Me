using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;

namespace KthulhuWantsMe.Source.Gameplay.Upgrades
{
    public class DamageUpgrade : IUpgrade
    {
        
        public UpgradeInfo UpgradeInfo => new UpgradeInfo()
        {
            Name = "Damage Upgrade",
            Description = "Increase damage by 10"
        };
        
        private readonly ThePlayer _player;
        private readonly int _value;

        public DamageUpgrade(ThePlayer player, int value)
        {
            _value = value;
            _player = player;
        }

        public void DoUpgrade()
        {
            _player.PlayerStats.Mods[StatType.BaseDamage] = _value;
        }
    }
}