using System.Collections;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private TentacleSpellCastingAbility _tentacleSpellCastingAbility;
        [SerializeField] private TentacleAggro _tentacleAggro;
       

        private float _attackCooldown;
        private bool _isAttacking;
        private bool _stunned;

        private float _livingTime;

        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        private void Update()
        {
            _livingTime += Time.deltaTime;
            
            if (BlockProcessing)
                return;

            UpdateAttackCooldown();
            DecideAttackStrategy();
        }

        public void ResetAI()
        {
            IsAttacking = false;
            Stunned = false;
            HoldsPlayer = false;
            BlockProcessing = false;
            
            _livingTime = 0f;
            
            ResetCooldown();
        }


        private void DecideAttackStrategy()
        {
            if (CanNotAttack())
                return;

            if (CanCastSpells())
            {
                if (_tentacleSpellCastingAbility.CanCastSpell(TentacleSpell.PlayerCantUseHealthItems))
                {
                    _tentacleSpellCastingAbility.CastSpell(TentacleSpell.PlayerCantUseHealthItems).Forget();

                    return;
                }
            }

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

        private bool CanCastSpells() => 
            _livingTime > _tentacleConfig.SpellActivationTime;
            

        private bool CanNotAttack() => 
            HoldsPlayer || Stunned || _tentacleSpellCastingAbility.CastingSpell;

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