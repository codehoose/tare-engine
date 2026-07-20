using System;

namespace TareMonoGameBridge.Extensions
{
    public static class IntegerExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            var v = Math.Max(value, min);
            v = Math.Min(v, max);
            return v;
        }
    }
}
