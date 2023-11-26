using System;

namespace KthulhuWantsMe.Source.Infrastructure
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T enumValue) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            int index = Array.IndexOf(values, enumValue);
            index = (index + 1) % values.Length;
            return values[index];
        }

        public static T Previous<T>(this T enumValue) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            int index = Array.IndexOf(values, enumValue);
            index = (index - 1 + values.Length) % values.Length;
            return values[index];
        }
    }
}