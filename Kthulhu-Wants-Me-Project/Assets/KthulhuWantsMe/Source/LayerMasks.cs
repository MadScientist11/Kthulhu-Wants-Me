using UnityEngine;

namespace KthulhuWantsMe.Source
{
    public static class LayerMasks
    {
        public static readonly int PlayerMask = LayerMask.GetMask(GameConstants.Layers.Player);
        public static readonly int EnemyMask = LayerMask.GetMask(GameConstants.Layers.Enemy);
        public static readonly int GroundMask = LayerMask.GetMask(GameConstants.Layers.Ground);
        public static readonly int IgnoreRaycastMask = LayerMask.GetMask("Ignore Raycast");
        public static readonly int All = Physics.AllLayers & ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        public static readonly int Nothing = 0;
        
        
        public static readonly int GroundLayer = LayerMask.NameToLayer(GameConstants.Layers.Ground);
  
    }
}