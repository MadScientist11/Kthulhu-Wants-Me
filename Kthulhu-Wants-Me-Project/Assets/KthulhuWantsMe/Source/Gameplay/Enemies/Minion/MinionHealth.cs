using KthulhuWantsMe.Source.Infrastructure.Services;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionHealth : Health
    {
        public override float MaxHealth => _minionConfig.MaxHealth;
        
        private MinionConfiguration _minionConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _minionConfig = dataProvider.MinionConfig;

        private void Start() => 
            RestoreHp();
    }
}