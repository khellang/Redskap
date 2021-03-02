using System;

namespace Redskap
{
    internal static class Checksum
    {
        internal static byte Mod11(ReadOnlySpan<byte> digits, ReadOnlySpan<byte> weights)
        {
            var rest = Sum(digits, weights) % 11;

            if (rest == 0)
            {
                return 0;
            }

            return (byte)(11 - rest);
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
