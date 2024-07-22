using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetTransparency(this Material material, int cachedColorProperty, float value)
        {
            Color color = material.GetColor(cachedColorProperty);
            color.a = value;
            material.SetColor(cachedColorProperty, color);
        }

        public static bool IsTransparent(this Material material)
        {
            return material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }
}