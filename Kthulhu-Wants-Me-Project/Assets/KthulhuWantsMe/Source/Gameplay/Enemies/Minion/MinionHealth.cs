using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Infrastructure.Services;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionHealth : Health
    {
        public override float MaxHealth => _cyaeghaConfig.MaxHealth;
        
        private CyaeghaConfiguration _cyaeghaConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _cyaeghaConfig = dataProvider.CyaeghaConfig;

        private void Start()
        {
            RestoreHp();
            Died += Die;
        }

        private void OnDestroy()
        {
            Died -= Die;
        }

        private void Die()
        {
            Destroy(gameObject, 2f);
        }
    }
}