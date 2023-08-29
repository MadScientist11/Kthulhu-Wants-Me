using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action Died;
        
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        
        public float Health = 100;

        private PlayerMovementController _movementController;
        
        private void Start()
        {
            _movementController = _playerLocomotion.MovementController;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
                return;
            }

            _playerAnimator.PlayImpact();
            _movementController.AddVelocity(-transform.forward * 30f);
        }

        private void Die()
        {
            _playerAnimator.PlayDie();
            Died?.Invoke();
        }
    }
}