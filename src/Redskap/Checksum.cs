using System;

namespace Redskap
{
    internal static class Checksum
    {
        public static int Mod11(ReadOnlySpan<byte> digits, ReadOnlySpan<byte> weights)
        {
            var rest = Sum(digits, weights) % 11;
            return rest == 0 ? 0 : 11 - rest;
        }

        private static int Sum(ReadOnlySpan<byte> digits, ReadOnlySpan<byte> weights)
        {
            var sum = 0;

            for (var i = 0; i < weights.Length; i++)
            {
                sum += digits[i] * weights[i];
            }

            return sum;
        }
    }
}
