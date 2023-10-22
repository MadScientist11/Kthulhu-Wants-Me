using UnityEngine;

namespace KthulhuWantsMe.Source
{
    public static class LayerMasks
    {
        public static readonly int PlayerMask = LayerMask.GetMask(GameConstants.Layers.Player);
        public static readonly int EnemyMask = LayerMask.GetMask(GameConstants.Layers.Enemy);
        public static readonly int GroundMask = LayerMask.GetMask(GameConstants.Layers.Ground);
        public static readonly int FadeableObjectMask = LayerMask.GetMask(GameConstants.Layers.FadeableObject);
        public static readonly int PlayerObstacleMask = ~(LayerMasks.PlayerMask & LayerMasks.GroundMask);
    }
}