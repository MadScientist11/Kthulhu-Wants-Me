using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAIBrain : MonoBehaviour
    {
        enum AttackDecision
        {
            BasicAttack = 0,
            GrabAbility = 1,
            SpellCast = 2,
            CastBuff = 3,
            Nothing = 100,
        }
        
        public bool BlockProcessing { get; set; }
        
        public bool Stunned
        {
            get => _stunned;
            set
            {
                _stunned = value;
                if (_stunned) 
                    StartCoroutine(StunWearOff());
            }
        }

        public bool SpecialTentacle => _specialTentacle;
        
        [SerializeField] private TentacleAttack _tentacleAttack;
        [SerializeField] private TentacleGrabAbility _tentacleGrabAbility;
        [SerializeField] private TentacleSpellCastingAbility _tentacleSpellCastingAbility;
        [SerializeField] private TentacleAggro _tentacleAggro;
        [SerializeField] private TentacleChase _tentacleChase;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        [SerializeField] private bool _specialTentacle;

        private float _livingTime;
        private float _reconsiderationTime;
        private bool _stunned;

        private TentacleConfiguration _tentacleConfiguration;


        private void Start()
        {
            _tentacleConfiguration = (TentacleConfiguration)_enemyStatsContainer.Config;
        }

        private void Update()
        {
            _livingTime += Time.deltaTime;
            
            if(BlockProcessing)
                return;
            
            DecideStrategy();
        }

        public void ResetAI()
        {
            _livingTime = 0;
            Stunned = false;
            BlockProcessing = false;
        }

        private void DecideStrategy()
        {
            if (ChasePlayer())
            {
                bool respawnSuccess = _tentacleChase.TryRespawn();
                
                if(respawnSuccess)
                    return;
            }
            
            DecideAttackStrategy();
        }

        private void DecideAttackStrategy()
        {
            _reconsiderationTime -= Time.deltaTime;

            if (CanNotAttack())
                return;


            _reconsiderationTime = _tentacleConfiguration.ReconsiderationTime;

            AttackDecision attackDecision = MakeAttackDecision();

            switch (attackDecision)
            {
                case AttackDecision.BasicAttack:
                    _tentacleAttack.PerformAttack();
                    break;
                case AttackDecision.GrabAbility:
                    _tentacleGrabAbility.GrabPlayer();
                    break;
                case AttackDecision.SpellCast:
                    _tentacleSpellCastingAbility.CastSpell(TentacleSpell.BasicAttackSpell).Forget();
                    break;
                case AttackDecision.CastBuff:
                    _tentacleSpellCastingAbility.CastSpell(TentacleSpell.Buff).Forget();
                    break;
                case AttackDecision.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DecideRespawn()
        {
            _tentacleChase.TryRespawn();
        }

        private AttackDecision MakeAttackDecision()
        {
            if (_specialTentacle && _tentacleSpellCastingAbility.CanCastSpell(TentacleSpell.Buff))
                return AttackDecision.CastBuff;
            else if (_specialTentacle)
                return AttackDecision.Nothing;


            //if (CanGrabPlayer())
            //    return AttackDecision.GrabAbility;
            if(CanDoBasicAttack())
                return AttackDecision.BasicAttack;
            else if(CanCastAttackSpell())
                return AttackDecision.SpellCast;
            else
                return AttackDecision.Nothing;
        }

        private IEnumerator StunWearOff()
        {
            yield return Utilities.WaitForSeconds.Wait(_tentacleConfiguration.StunWearOffTime);
            Stunned = false;
        }

        private bool CanGrabPlayer() =>
            Random.value < _tentacleConfiguration.GrabAbilityChance && _tentacleAggro.HasAggro;

        private bool CanDoBasicAttack() =>
            _tentacleAttack.CanAttack() && _tentacleAggro.HasAggro && _tentacleAggro.IsPlayerInFront && _tentacleAggro.DistanceToPlayer() < _tentacleConfiguration.AttackActivationDistance;
        
        private bool CanCastAttackSpell() =>
            _tentacleSpellCastingAbility.CanCastSpell(TentacleSpell.BasicAttackSpell) && _tentacleAggro.HasAggro && _tentacleAggro.DistanceToPlayer() > _tentacleConfiguration.SpellAttackActivationDistance;

        private bool CanNotAttack() =>
            _tentacleGrabAbility.HoldsPlayer || _tentacleAttack.IsAttacking || _reconsiderationTime > 0 || Stunned;

        private bool ChasePlayer() =>
            _tentacleChase != null && _tentacleChase.CanChase();

    }
}