using System;

namespace Rovio.Common
{
    public static class Utils
    {
        public static T Clamp<T>(T value, T min, T max)
                where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            else if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
    }
}
