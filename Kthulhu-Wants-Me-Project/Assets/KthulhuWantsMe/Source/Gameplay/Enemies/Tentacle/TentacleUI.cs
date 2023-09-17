using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.UI;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
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
            _tentacleHealth.TookDamage += _hpBar.gameObject.SwitchOn;
            _tentacleHealth.Died += _hpBar.gameObject.SwitchOff;
            _hpBar.gameObject.SwitchOff();
        }

  
        private void OnDestroy()
        {
            _tentacleHealth.Changed -= UpdateHpBar;
            _tentacleHealth.TookDamage -= _hpBar.gameObject.SwitchOn;
            _tentacleHealth.Died -= _hpBar.gameObject.SwitchOff;
        }

        private void UpdateHpBar(float newValue)
        {
            _hpBar.SetValue(newValue, _tentacleConfig.MaxHealth);
        }
    }
}