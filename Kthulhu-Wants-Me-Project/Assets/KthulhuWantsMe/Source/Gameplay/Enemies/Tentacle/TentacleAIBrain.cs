using System.Collections;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAIBrain : MonoBehaviour
    {
        public bool IsAttacking
        {
            get => _isAttacking;
            set
            {
                _isAttacking = value;
                
                if (!_isAttacking)
                    ResetCooldown();
            }
        }

        public bool Stunned
        {
            get => _stunned;
            set
            {
                _stunned = value;
                if (_stunned)
                {
                    StartCoroutine(StunWearOff());
                }
            }
        }

        public bool HoldsPlayer { get; set; }

        public bool BlockProcessing { get; set; }
        

        [SerializeField] private TentacleAttack _tentacleAttack;
        [SerializeField] private TentacleGrabAbility _tentacleGrabAbility;
        [SerializeField] private TentacleAggro _tentacleAggro;
        [SerializeField] private TentacleEmergence _tentacleEmergence;
        [SerializeField] private TentacleRetreat _tentacleRetreat;

        private float _attackCooldown;
        private bool _isAttacking;
        private bool _stunned;

        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        private void Update()
        {
            if (BlockProcessing)
                return;

            UpdateAttackCooldown();
            DecideAttackStrategy();
        }

        public void ResetBrain()
        {
            IsAttacking = false;
            Stunned = false;
            HoldsPlayer = false;
            BlockProcessing = false;
            ResetCooldown();
        }

        private void DecideAttackStrategy()
        {
            if (CanNotAttack())
                return;


            if (GrabAbilityConditionsFulfilled())
            {
                _tentacleGrabAbility.GrabPlayer();
                return;
            }

       
            if (CanDoBasicAttack())
                _tentacleAttack.PerformAttack();
        }

        private IEnumerator StunWearOff()
        {
            yield return new WaitForSeconds(_tentacleConfig.StunWearOffTime);
            Stunned = false;
            ResetCooldown();
        }

        private bool CanNotAttack() => 
            HoldsPlayer || Stunned;

        private bool CanDoBasicAttack() =>
            CooldownIsUp() && !IsAttacking && _tentacleAggro.HasAggro;

        private void UpdateAttackCooldown() =>
            _attackCooldown -= Time.deltaTime;

        private bool CooldownIsUp() =>
            _attackCooldown <= 0;

        private void ResetCooldown() =>
            _attackCooldown = _tentacleConfig.AttackCooldown;

        private bool GrabAbilityConditionsFulfilled() => 
            Random.value < _tentacleConfig.GrabAbilityChance && CanDoBasicAttack();
    }
}