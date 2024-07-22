using System.Text.RegularExpressions;
using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities.Extensions
{
    public static class Extensions
    {
        public static Vector3 AddY(this Vector3 vec, float offset)
        {
            return new Vector3(vec.x, vec.y + offset, vec.z);
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}