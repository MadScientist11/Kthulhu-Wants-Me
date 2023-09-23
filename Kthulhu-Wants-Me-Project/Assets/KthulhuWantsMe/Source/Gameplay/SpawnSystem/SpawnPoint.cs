using System;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SpawnSystem
{
    public class SpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        
        public SpawnPointType SpawnPointType;
        
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gizmoSize;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(transform.position, _gizmoSize);
        }
    }
}
