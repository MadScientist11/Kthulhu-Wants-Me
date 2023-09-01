using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.UI;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleUI : MonoBehaviour
    {
        [SerializeField] private TentacleHealth _tentacleHealth;
        [SerializeField] private HpBar _hpBar;
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        private void Awake()
        {
            _tentacleHealth.Changed += UpdateHpBar;
        }

        private void OnDestroy()
        {
            _tentacleHealth.Changed -= UpdateHpBar;
        }

        private void UpdateHpBar(float newValue)
        {
            _hpBar.SetValue(newValue, _tentacleConfig.MaxHealth);
        }
    }
}