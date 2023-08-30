using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    [Serializable]
    public class PortalZone
    {
        public Quaternion Rotation;
        public Matrix4x4 LocalToWrold;
    }
}