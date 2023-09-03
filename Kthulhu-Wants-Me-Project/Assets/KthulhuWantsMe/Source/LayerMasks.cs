using UnityEngine;

namespace KthulhuWantsMe.Source
{
    public static class LayerMasks
    {
        public static readonly int PlayerMask = LayerMask.GetMask(GameConstants.Layers.Player);
        public static readonly int EnemyMask = LayerMask.GetMask(GameConstants.Layers.Enemy);
    }
}