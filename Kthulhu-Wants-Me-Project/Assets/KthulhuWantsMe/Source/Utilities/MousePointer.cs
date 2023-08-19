using Unity.Mathematics;
using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities
{
    public static class MousePointer
    {
        public static float2 GetScreenPosition(bool useOldSystem = true)
        {
            if (useOldSystem)
            {
                Vector3 posn = Input.mousePosition;
                return new float2(posn.x, posn.y);
            }
            else
            {
                return 0;
            }
        }

        public static float2 GetBoundedScreenPosition(bool useOldSystem = true)
        {
            float2 raw = GetScreenPosition(useOldSystem);
            return math.clamp(raw, new float2(0, 0), new float2(Screen.width - 1, Screen.height - 1));
        }

        public static float2 GetViewportPosition(bool useOldSystem = true)
        {
            float2 screenPos = GetScreenPosition(useOldSystem);
            return screenPos / new float2(Screen.width, Screen.height);
        }

        public static float2 GetViewportPosition(Camera camera, bool useOldSystem = true)
        {
            float2 screenPos = GetScreenPosition(useOldSystem);
            float3 viewportPos = camera.ScreenToViewportPoint(new float3(screenPos, 0));
            return viewportPos.xy;
        }

        public static float3 GetWorldPosition(Camera camera, bool useOldSystem = true)
        {
            return GetWorldPosition(camera, camera.nearClipPlane, useOldSystem);
        }

        public static float3 GetWorldPosition(Camera camera, float worldDepth, bool useOldSystem = true)
        {
            float2 screenPos = GetBoundedScreenPosition(useOldSystem);
            float3 screenPosWithDepth = new float3(screenPos, worldDepth);
            return camera.ScreenToWorldPoint(screenPosWithDepth);
        }

        public static Ray GetWorldRay(Camera camera, bool useOldSystem = true)
        {
            float2 screenPos = GetBoundedScreenPosition(useOldSystem);
            float3 screenPosWithDepth = new float3(screenPos, camera.nearClipPlane);
            return camera.ScreenPointToRay(screenPosWithDepth);
        }
    }
}