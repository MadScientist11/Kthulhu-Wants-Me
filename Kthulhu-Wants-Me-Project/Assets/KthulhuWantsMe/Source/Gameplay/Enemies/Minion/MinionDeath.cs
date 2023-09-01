using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionDeath : MonoBehaviour
    {
        [SerializeField] private MinionHealth _enemyHealth;
        [SerializeField] private MinionAIBrain _minionAIBrain;

        private void Start()
        {
            _enemyHealth.Died += OnDied;
        }

        private void OnDestroy()
        {
            _enemyHealth.Died -= OnDied;
        }

        private void OnDied()
        {
            _minionAIBrain.BlockProcessing = true;
        }
    }
}