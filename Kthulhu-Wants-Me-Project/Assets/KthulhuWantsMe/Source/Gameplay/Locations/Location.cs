using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    public enum LocationId
    {
        GameLocation = 0,
        TestLocation = 1,
        Unknown = 10000,
    }
    [CreateAssetMenu(menuName = "Create Location", fileName = "Location", order = 0)]
    public class Location : ScriptableObject
    {
        public LocationId LocationId;
        public Vector3 PlayerSpawnPosition;
        public Quaternion PlayerSpawnRotation;
        public List<PortalZone> PortalSpawnZones;
        public List<EnemySpawnZoneData> EnemySpawnZones;

#if UNITY_EDITOR
        [Button]
        public void CollectInfoFromCurrentScene()
        {
            PlayerSpawnPosition = FindObjectOfType<PlayerSpawnPoint>().Position;
            PlayerSpawnRotation = FindObjectOfType<PlayerSpawnPoint>().Rotation;
            PortalSpawnZones = FindObjectsOfType<PortalSpawnZone>().Select(enemySp => new PortalZone()
            {
                Rotation = enemySp.Rotation,
                LocalToWrold = enemySp.transform.localToWorldMatrix,
            }).ToList();

            EnemySpawnZones = FindObjectsOfType<EnemySpawnZone>().Select(enemySp => new EnemySpawnZoneData()
            {
                Position = enemySp.Position,
                Radius = enemySp.Radius,
            }).ToList();

            EditorUtility.SetDirty(this);
        }
#endif
    }
}