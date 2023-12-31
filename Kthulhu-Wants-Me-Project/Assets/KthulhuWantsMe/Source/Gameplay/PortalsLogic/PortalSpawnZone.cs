﻿using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public class PortalSpawnZone : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        public Vector3 Scale => transform.localScale;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}