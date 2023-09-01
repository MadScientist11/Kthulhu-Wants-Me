using KthulhuWantsMe.Source.Infrastructure.Services;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => _tentacleConfig.MaxHealth;
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _tentacleConfig = dataProvider.TentacleConfig;
    }
}