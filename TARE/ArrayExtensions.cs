using System;
using System.Linq;

namespace TARE
{
    internal static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] array, int start, int length)
        {
            var temp = new T[length];
            Array.Copy(array, start, temp, 0, length);
            return temp;
        }

        public static string ToText(this char[] array)
        {
            return new string(array.Select(ch => ch == 0 ? ' ' : ch).ToArray(), 0, array.Length);
        }
    }
}
