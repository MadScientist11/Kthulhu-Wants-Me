using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells;
using KthulhuWantsMe.Source.Infrastructure.Services;
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
            Nothing = 3,
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
        
        [SerializeField] private TentacleAttack _tentacleAttack;
        [SerializeField] private TentacleGrabAbility _tentacleGrabAbility;
        [SerializeField] private TentacleSpellCastingAbility _tentacleSpellCastingAbility;
        [SerializeField] private TentacleAggro _tentacleAggro;

        private float _livingTime;
        private float _reconsiderationTime;
        private bool _stunned;

        public const float ReconsiderationTime = 1f;

        private TentacleConfiguration _tentacleConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider) =>
            _tentacleConfiguration = dataProvider.TentacleConfig;

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

        private void DecideStrategy() => 
            DecideAttackStrategy();

        private void DecideAttackStrategy()
        {
            _reconsiderationTime -= Time.deltaTime;
            
            if (CanNotAttack())
                return;


            _reconsiderationTime = ReconsiderationTime;

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
                case AttackDecision.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AttackDecision MakeAttackDecision()
        {
            float decisionValue = Random.value;

            if (CanGrabPlayer() && decisionValue < _tentacleConfiguration.GrabAbilityChance)
                return AttackDecision.GrabAbility;
            else if(CanDoBasicAttack())
                return AttackDecision.BasicAttack;
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
            _tentacleAttack.CanAttack() && _tentacleAggro.HasAggro;

        private bool CanNotAttack() =>
            _tentacleGrabAbility.HoldsPlayer || _tentacleAttack.IsAttacking || _reconsiderationTime > 0 || Stunned;


  
    }
}