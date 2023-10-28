using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player.State
{
    public class PlayerStats
    {
        public event Action<StatType, float> StatChanged;
        
        public float CurrentHp;
        public float CurrentStamina;
        public bool Immortal;

        public IReadOnlyDictionary<StatType, float> MainStats
        {
            get
            {
                return _mainStats;
            }
        }

        public List<SkillId> AcquiredSkills { get; }


        private readonly Dictionary<StatType, float> _mainStats;


        private PlayerConfiguration _playerConfiguration;

        public PlayerStats(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
            _mainStats = playerConfiguration.BaseStats.ToDictionary(entry => entry.Key,
                entry => entry.Value);
            AcquiredSkills = new();
        }

        public void ChangeStat(StatType statType, float newValue)
        {
            if (statType == StatType.MaxHealth)
            {
                newValue = Mathf.Floor(newValue);
            }
            _mainStats[statType] = newValue;
            StatChanged?.Invoke(statType, _mainStats[statType]);
        }
    }
}