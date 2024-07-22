using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.Stats;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.Utilities.Extensions;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class HealthBarHUD : MonoBehaviour
    {
        [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private Health _health;
        [SerializeField] private HpBar _hpBar;
        
        private PlayerFacade _player;

        [Inject]
        public void Construct(IPlayerProvider playerProvider)
        {
            _player = playerProvider.Player;
        }

        private void Awake()
        {
            _health.Changed += UpdateHpBar;
            _health.TookDamage += _hpBar.gameObject.SwitchOn;
            _health.Died += _hpBar.gameObject.SwitchOff;
            _hpBar.gameObject.SwitchOff();
        }

        private void OnDestroy()
        {
            _health.Changed -= UpdateHpBar;
            _health.TookDamage -= _hpBar.gameObject.SwitchOn;
            _health.Died -= _hpBar.gameObject.SwitchOff;
        }

        private void Update()
        {
            transform.LookAt(_player.PlayerVirtualCamera.transform);
        }

        private void UpdateHpBar(float newValue)
        {
            _hpBar.SetValue(newValue, enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth]);
        }
    }
}