﻿using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.EnemyScaling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemyConfiguration", fileName = "EnemyConfiguration", order = 0)]
    public class EnemyConfiguration : SerializedScriptableObject
    {
        public EnemyType EnemyType;
        public GameObject Prefab;
        public Dictionary<StatType, float> BaseStats = new();
        public EnemyScalingSO EnemyScaling;
        
        public float Knockback = 10;
        public float KnockbackTime = .25f;
        
        [MinMaxSlider(0, 20, true)]
        public Vector2 MoveSpeed = new(3,4);

        public bool IsElite()
        {
            return EnemyType == EnemyType.Tentacle || EnemyType == EnemyType.BleedTentacle ||
                    EnemyType == EnemyType.PoisonousTentacle || EnemyType == EnemyType.TentacleSpecial;
        }
    }
}