using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithHealth : Health
    {
        public override float MaxHealth => _yithConfiguration.MaxHealth;

        [SerializeField] private MMFeedbacks _hitFeedbacks;
        
        private YithConfiguration _yithConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _yithConfiguration = dataProvider.YithConfig;
        }

        public void HandleDeath()
        {
            _hitFeedbacks.PlayFeedbacks();
            Destroy(gameObject, 2f);
        }
    }
}