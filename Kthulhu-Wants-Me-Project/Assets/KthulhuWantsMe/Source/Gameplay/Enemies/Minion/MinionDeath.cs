using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private MinionAIBrain _minionAIBrain;

        private void Start()
        {
            _enemyHealth.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _enemyHealth.OnDied -= OnDied;
        }

        private void OnDied()
        {
            _minionAIBrain.BlockProcessing = true;
        }
    }
}