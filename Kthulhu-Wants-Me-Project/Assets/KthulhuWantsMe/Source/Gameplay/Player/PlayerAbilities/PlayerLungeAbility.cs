using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerLungeAbility : MonoBehaviour, IAbility
    {
        public bool IsInLunge => _lunge || _lungeCharge;
        
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private PlayerAnimator _playerAnimator;

        private const float LungeChargeTime = 5f;
        private const float LungeChargeMaxVelocity = 100f;
        private const float MinLungeChargeVelocity = 30;

        private bool _lungeCharge;
        private bool _lunge;
        private float _chargeVelocityStep;

        private float _chargedVelocity;
        
        
        private IInputService _inputService;
        private PlayerConfiguration _playerConfiguration;
        private ThePlayer _player;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, ThePlayer player)
        {
            _player = player;
            _playerConfiguration = dataProvider.PlayerConfig;
            _inputService = inputService;

            _playerHealth.TookDamage += CancelLungeCharge;
            _inputService.GameplayScenario.LungeHold += OnLungeInitiated;
            _inputService.GameplayScenario.LungePerformed += OnLunge;
        }

        private void OnDestroy()
        {
            _playerHealth.TookDamage -= CancelLungeCharge;
            _inputService.GameplayScenario.LungeHold -= OnLungeInitiated;
            _inputService.GameplayScenario.LungePerformed -= OnLunge;
        }

        private void Start()
        {
            _chargeVelocityStep = LungeChargeMaxVelocity / LungeChargeTime;
        }

        private void Update()
        {
            if (_lungeCharge)
            {
                _chargedVelocity += _chargeVelocityStep * Time.deltaTime;
                _playerLocomotion.FaceMouse();
            } 

            if (_lunge) 
                ProcessLunge();
        }

        private void OnLungeInitiated()
        {
            if(!_player.PlayerStats.AcquiredSkills.Contains(SkillId.Lunge))
                return;
            
            _lungeCharge = true;
            _playerAnimator.PlayLungeCharge();
        }

        private void OnLunge()
        {
            if(!_player.PlayerStats.AcquiredSkills.Contains(SkillId.Lunge))
                return;
            
            if (_chargedVelocity < MinLungeChargeVelocity)
            {
                CancelLungeCharge();
                return;
            }

            ApplyLungeVelocity();
            CancelLungeCharge();

            _playerAnimator.PlayLunge();
            _lunge = true;
        }

        private void ProcessLunge()
        {
            if (LungeInProcess())
            {
                if (PhysicsUtility.HitMany( AttackStartPoint(), _playerConfiguration.LungeRadius, LayerMasks.EnemyMask,
                        out List<IDamageable> enemies))
                {
                    foreach (IDamageable enemy in enemies) 
                        enemy.TakeDamage(_playerConfiguration.LungeBaseDamage);
                    
                    _playerLocomotion.MovementController.KillVelocity();
                }
            }
            else
            {
                _lunge = false;
            }
        }

        private void ApplyLungeVelocity() =>
            _playerLocomotion.MovementController.AddVelocity(transform.forward * _chargedVelocity);

        private void CancelLungeCharge()
        {
            _lungeCharge = false;
            _chargedVelocity = 0;
            _playerAnimator.StopLungeCharge();
        }

        private bool LungeInProcess()
        {
            return _playerLocomotion.MovementController.CurrentVelocity.sqrMagnitude > 0.1f ||
                   _playerLocomotion.MovementController.InternalVelocityAdd.sqrMagnitude > 0;
        }

        private Vector3 AttackStartPoint() =>
            new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) +
            transform.forward * 1;
    }
}