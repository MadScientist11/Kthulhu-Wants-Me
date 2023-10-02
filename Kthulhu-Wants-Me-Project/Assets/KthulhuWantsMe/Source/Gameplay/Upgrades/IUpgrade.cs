namespace KthulhuWantsMe.Source.Gameplay.Upgrades
{
    public interface IUpgrade
    {
        UpgradeInfo UpgradeInfo { get; }
        void DoUpgrade();
    }
}