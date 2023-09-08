using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class StatsUI : MonoBehaviour
    {
        public TextMeshProUGUI HpStatText;
        public TextMeshProUGUI AttackStatText;
        
        private IPlayerStats _playerStats;

        [Inject]
        public void Construct(IPlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        private void Update()
        {
            HpStatText.text = $"HP: {_playerStats.Stats.Health}";
            AttackStatText.text = $"ATK: {_playerStats.GetOverallDamage()}";
        }
    }
}
