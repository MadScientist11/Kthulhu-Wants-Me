using System;
using UnityEngine;

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
        
        private void Update()
        {
            if(BlockProcessing)
                return;
            
            if (HasAggro) 
                _minionFollow.FollowPlayer();
            
            if(CanAttack())
                _minionAttack.PerformAttack();
            
        }

        private bool CanAttack() => 
            AttackCooldownReached && IsInAttackRange && !IsAttacking;
    }
}