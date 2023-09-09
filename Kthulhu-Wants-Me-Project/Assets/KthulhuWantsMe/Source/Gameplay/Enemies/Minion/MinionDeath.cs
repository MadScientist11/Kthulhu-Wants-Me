using System;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionDeath : MonoBehaviour
    {
        [SerializeField] private MinionHealth _enemyHealth;
        [FormerlySerializedAs("_minionAIBrain")] [SerializeField] private YithAIBrain yithAIBrain;

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
           // yithAIBrain.BlockProcessing = true;
        }
    }
}