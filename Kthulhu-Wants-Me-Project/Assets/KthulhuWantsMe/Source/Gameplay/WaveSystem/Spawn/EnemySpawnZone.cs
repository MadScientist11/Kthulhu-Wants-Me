using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn
{
    [Serializable]
    public class EnemySpawnZoneData
    {
        public Vector3 Position;
        public float Radius;
    }
    public class EnemySpawnZone : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public float Radius;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1,0,0,0.8f);
            Gizmos.DrawSphere(Position, Radius);
        }
    }
}
