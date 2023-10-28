using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
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
        public override float MaxHealth => _thePlayer.MaxHealth;

        public override float CurrentHealth => _thePlayer.CurrentHp;

        [SerializeField] private PlayerFacade _player;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerDashAbility _playerDashAbility;
        [SerializeField] private EntityBuffDebuffContainer _entityBuffDebuffContainer;
        [SerializeField] private MMFeedbacks _healFeedback;
        [SerializeField] private MMFeedbacks _invincibilityFeedback;

        private PlayerMovementController _movementController;
        
        private IInputService _inputService;
        private ThePlayer _thePlayer;
        private ICoroutineRunner _coroutineRunner;


        [Inject]
        public void Construct(IInputService inputService, ThePlayer thePlayer, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _thePlayer = thePlayer;
            _inputService = inputService;
            
            _thePlayer.TookDamage += OnTookDamage;
            _thePlayer.HealthChanged += OnHealthChanged;
            _thePlayer.Died += OnDied;
        }

        private void OnDestroy()
        {
            _thePlayer.TookDamage -= OnTookDamage;
            _thePlayer.HealthChanged -= OnHealthChanged;
            _thePlayer.Died -= OnDied;
        }

        private void Start()
        {
            _movementController = _playerLocomotion.MovementController;
        }
        
        public override void TakeDamage(float damage, IDamageProvider damageProvider)
        {
            _thePlayer.TakeDamage(damageProvider);
        }

        public override void TakeDamage(float damage)
        {
            _thePlayer.TakeDamage(new Damage(damage));
        }

        public override void Heal(float amount)
        {
            _thePlayer.Heal(amount);
        }

        private void OnImpact()
        {
            _player.ChangePlayerLayer(LayerMask.NameToLayer(GameConstants.Layers.PlayerRoll));
            _inputService.GameplayScenario.Disable();
        }

        private void OnImpactEnd()
        {
            _coroutineRunner.ExecuteAfter(1f,() =>
                _player.ChangePlayerLayer(LayerMask.NameToLayer(GameConstants.Layers.Player)));
            _inputService.GameplayScenario.Enable();
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
            _playerDashAbility.ResetEvade();
            _invincibilityFeedback?.PlayFeedbacks();
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