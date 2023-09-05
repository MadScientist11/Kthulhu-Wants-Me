using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAIBrain : MonoBehaviour
    {
        public bool HasAggro { get; set; }
        public bool IsAttacking { get; set; }
        public bool AttackCooldownReached { get; set; }
        public bool IsInAttackRange { get; set; }
        public bool BlockProcessing { get; set; }

        [SerializeField] private MinionFollow _minionFollow;
        [SerializeField] private MinionAttack _minionAttack;

        private float _attackDelay;
        
        private MinionConfiguration _minionConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _minionConfiguration = dataProvider.MinionConfig;
        }
        
        private void Update()
        {
            if(BlockProcessing)
                return;
            
            if (HasAggro) 
                _minionFollow.FollowPlayer();

            if (CanAttack())
            {
                _attackDelay -= Time.deltaTime;
                if (_attackDelay <= 0)
                {
                    _attackDelay = _minionConfiguration.AttackDelay;
                    _minionAttack.PerformAttack();
                }

            }
            
        }

        private bool CanAttack() => 
            AttackCooldownReached && IsInAttackRange && !IsAttacking;
    }
}