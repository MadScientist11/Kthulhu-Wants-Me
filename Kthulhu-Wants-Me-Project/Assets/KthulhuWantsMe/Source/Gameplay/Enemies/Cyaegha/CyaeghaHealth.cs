using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaHealth : Health
    {
        public override float MaxHealth => _cyaeghaConfiguration.MaxHealth;
        
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        
        private CyaeghaConfiguration _cyaeghaConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _cyaeghaConfiguration = dataProvider.CyaeghaConfig;

            Died += HandleDeath;
        }

        private void OnDestroy()
        {
            Died -= HandleDeath;
        }

        private void HandleDeath()
        {
            _hitFeedbacks?.PlayFeedbacks();
            Destroy(gameObject, 2f);
        }
    }
}