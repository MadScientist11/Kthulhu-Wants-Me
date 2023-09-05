using System.Collections.Generic;
using System.Linq;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    [CreateAssetMenu(menuName = "Create Location", fileName = "Location", order = 0)]
    public class Location : ScriptableObject
    {
        public Vector3 PlayerSpawnPosition;
        public Quaternion PlayerSpawnRotation;
        public List<PortalZone> PortalSpawnZones;


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
            
            EditorUtility.SetDirty(this);
        }
    }
}