using System;

namespace KthulhuWantsMe.Source.Utilities
{
    public class EnumExtensions<T> where T : struct, IConvertible
    {
        public static T Random
        {
            get
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                Array values = Enum.GetValues(typeof(T));
                int randomValue =  UnityEngine.Random.Range(0, values.Length);;
                return (T)values.GetValue(randomValue);
            }
        }
    }
}