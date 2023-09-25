using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.UI;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleUI : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private TentacleHealth _tentacleHealth;
        [SerializeField] private HpBar _hpBar;
        
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
            _hpBar.SetValue(newValue, _enemy.EnemyStats.Stats[StatType.MaxHealth]);
        }
    }
}