namespace KthulhuWantsMe.Source.Gameplay.UpgradeSystem
{
    public class StatUpgrade : IUpgrade
    {
        private UpgradeData _upgrade;

        public StatUpgrade(UpgradeData upgrade)
        {
            _upgrade = upgrade;
        }

        public void DoUpgrade()
        {
        }
    }
}