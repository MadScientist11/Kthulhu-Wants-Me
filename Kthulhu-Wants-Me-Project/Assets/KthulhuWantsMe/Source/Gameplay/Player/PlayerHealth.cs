using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerHealth : Health
    {
        public override float MaxHealth => _playerConfiguration.MaxHealth;
        
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        
        private PlayerMovementController _movementController;

        private PlayerConfiguration _playerConfiguration;
        
        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _playerConfiguration = dataProvider.PlayerConfig;
        
        private void Start() => 
            _movementController = _playerLocomotion.MovementController;

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            if (CurrentHealth <= 0)
            {
                Die();
                return;
            }

            ReceiveDamageVisual();
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