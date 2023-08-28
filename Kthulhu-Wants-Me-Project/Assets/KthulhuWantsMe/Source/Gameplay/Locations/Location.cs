﻿using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Player;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    [CreateAssetMenu(menuName = "Create Location", fileName = "Location", order = 0)]
    public class Location : ScriptableObject
    {
        public Vector3 PlayerSpawnPosition;
        public List<LocationEnemyData> Enemies;


        [Button]
        public void CollectInfoFromCurrentScene()
        {
            PlayerSpawnPosition = FindObjectOfType<PlayerSpawnPoint>().Position;
            Enemies = FindObjectsOfType<EnemySpawnPoint>().Select(enemySp => new LocationEnemyData
            {
                Position = enemySp.Position,
                EnemyType = enemySp.EnemyType,
            }).ToList();
            
            EditorUtility.SetDirty(this);
        }
    }
}