using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerHealth : Health
    {
        public override float MaxHealth => _playerStats.Stats.MaxHealth;

        public override float CurrentHealth
        {
            get { return _playerStats.Stats.Health;  }
            protected set
            {
                float newHealth = Mathf.Clamp(value, 0, MaxHealth);
                
                
                if (newHealth < _playerStats.Stats.Health)
                    RaiseTookDamageEvent();

                _playerStats.Stats.Health = newHealth;

                RaiseHealthChangedEvent(_playerStats.Stats.Health);

                if (_playerStats.Stats.Health == 0)
                    RaiseDiedEvent();
            }
        }

        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private MMFeedbacks _healFeedback;
        
        private PlayerMovementController _movementController;

        private IPlayerStats _playerStats;
        
        [Inject]
        public void Construct(IPlayerStats playerStats) => 
            _playerStats = playerStats;

        private void Start()
        {
            _movementController = _playerLocomotion.MovementController;
            RestoreHp();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            Debug.Log($"Player took {damage}");
            if (CurrentHealth <= 0)
            {
                Die();
                return;
            }

            ReceiveDamageVisual();
        }

        public override void Heal(float amount)
        {
            float prevHealth = CurrentHealth;
            base.Heal(amount);
            if (CurrentHealth > prevHealth)
            {
                _healFeedback.PlayFeedbacks(transform.position);
            }
        }

        private void ReceiveDamageVisual()
        {
            _playerAnimator.PlayImpact();
            _playerLocomotion.BlockMovement(.5f);
            AddKnockback();
            _movementController.KillVelocity();
        }

        private void AddKnockback() => 
            _movementController.AddVelocity(-transform.forward * 30f);

        private void Die()
        {
            _playerAnimator.PlayDie();
            _movementController.ToggleMotor(false);
        }
    }
}