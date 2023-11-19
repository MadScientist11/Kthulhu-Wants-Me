using KthulhuWantsMe.Source.Gameplay.Player.State;

namespace KthulhuWantsMe.Source.Gameplay.UpgradeSystem
{
    public class SkillAcquirementUpgrade : IUpgrade
    {
        private readonly UpgradeData _upgrade;
        private readonly ThePlayer _player;

        public SkillAcquirementUpgrade(UpgradeData upgrade, ThePlayer player)
        {
            _player = player;
            _upgrade = upgrade;
        }

        public void DoUpgrade()
        {
            _player.PlayerStats.AcquiredSkills.Add(_upgrade.SkillId);
        }
    }
}