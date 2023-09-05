using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public static class DataExtensions
    {
        public static TentacleSettings AsRuntimeSettings(this TentacleConfiguration tentacleConfiguration)
        {
            TentacleSettings tentacleSettings = new TentacleSettings();
            tentacleSettings.BaseDamage = tentacleConfiguration.BaseDamage;
            tentacleSettings.TentacleGrabDamage = tentacleConfiguration.TentacleGrabDamage;
            tentacleSettings.MaxHealth = tentacleConfiguration.MaxHealth;
            return tentacleSettings;
        }
    }
}