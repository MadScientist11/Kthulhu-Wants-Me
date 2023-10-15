using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class Damage : IDamageProvider
    {
        private readonly float _damage;

        public Damage(float damage)
        {
            _damage = damage;
        }

        public Transform DamageDealer { get; }

        public float ProvideDamage() => 
            _damage;
    }
    public class PlayerHealth : Health
    {
        public override float MaxHealth => _player.MaxHealth;

        public override float CurrentHealth => _player.CurrentHp;

        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private EntityBuffDebuffContainer _entityBuffDebuffContainer;
        [SerializeField] private MMFeedbacks _healFeedback;

        private PlayerMovementController _movementController;

        private ThePlayer _player;

        [Inject]
        public void Construct(ThePlayer player)
        {
            _player = player;
            _player.TookDamage += OnTookDamage;
            _player.HealthChanged += OnHealthChanged;
            _player.Died += OnDied;
        }

        private void OnDestroy()
        {
            _player.TookDamage -= OnTookDamage;
            _player.HealthChanged -= OnHealthChanged;
            _player.Died -= OnDied;
        }

        private void Start()
        {
            _movementController = _playerLocomotion.MovementController;
        }
        
        public override void TakeDamage(float damage, IDamageProvider damageProvider)
        {
            _player.TakeDamage(damageProvider);
        }

        public override void TakeDamage(float damage)
        {
            _player.TakeDamage(new Damage(damage));
        }

        public override void Heal(float amount)
        {
            _player.Heal(amount);
        }

        private void OnTookDamage(IDamageProvider damageProvider)
        {
            RaiseTookDamageEvent();
            
            if (damageProvider is IBuffDebuff)
                return;
            ReceiveDamageVisual(damageProvider);
        }

        private void OnHealthChanged(HealthChange healthChange)
        {
            RaiseHealthChangedEvent(healthChange.Current);
            if (healthChange.Current > healthChange.Previous)
            {
                //_healFeedback.PlayFeedbacks(transform.position);
            }
        }

        private void OnDied()
        {
            RaiseDiedEvent();
            Die();
        }
        
        private void ReceiveDamageVisual(IDamageProvider damageProvider)
        {
            _playerAnimator.PlayImpact();
            _playerAttack.ResetAttackState();
            _playerLocomotion.BlockMovement(.5f);
            AddKnockback(damageProvider.DamageDealer);
            _movementController.KillVelocity();
        }

        private void AddKnockback(Transform damageDealer)
        {
            if(damageDealer != null)
                _movementController.AddVelocity(damageDealer.forward * 30f);
        }

        private void Die()
        {
            _playerAnimator.PlayDie();
            _movementController.ToggleMotor(false);
        }

        
    }
}