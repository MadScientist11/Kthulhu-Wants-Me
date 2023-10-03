﻿using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player.State
{
    public class PlayerStats
    {
        public float CurrentHp;
        public bool Immortal;
        public float EvadeDelay;
        public Dictionary<StatType, float> BaseStats;
        public Dictionary<StatType, float> Mods = new();
        

        private PlayerConfiguration _playerConfiguration;

        public PlayerStats(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
            BaseStats = playerConfiguration.BaseStats.ToDictionary(entry => entry.Key,
                entry => entry.Value);

            EvadeDelay = _playerConfiguration.DashCooldown;
        }

       
    }
}