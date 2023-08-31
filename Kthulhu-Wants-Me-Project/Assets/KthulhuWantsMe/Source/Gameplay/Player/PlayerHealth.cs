using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action Died;
        
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        
        private PlayerMovementController _movementController;
        private PlayerConfiguration _playerConfiguration;

        private float _currentHealth;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _playerConfiguration = dataProvider.PlayerConfig;
        }
        
        private void Start()
        {
            _movementController = _playerLocomotion.MovementController;
            _currentHealth = _playerConfiguration.MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            Debug.Log(_currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
                return;
            }

            ReceiveDamage();
        }

        private void ReceiveDamage()
        {
            _playerAnimator.PlayImpact();
            _playerLocomotion.BlockMovement(.5f);
            AddKnockback();
            _movementController.KillVelocity();
        }

        private void AddKnockback()
        {
            _movementController.AddVelocity(-transform.forward * 30f);
        }
        
        private void Die()
        {
            _playerAnimator.PlayDie();
            _movementController.ToggleMotor(false);
            Died?.Invoke();
        }
    }
}