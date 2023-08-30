using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    [Serializable]
    public class PortalZone
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector2 Extents;
        public Matrix4x4 LocalToWrold;
        public Vector3 Normal;
    }
}