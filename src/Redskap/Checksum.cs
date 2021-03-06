using System;

namespace Redskap
{
    internal static class Checksum
    {
        internal static byte Mod11(ReadOnlySpan<char> value, ReadOnlySpan<byte> weights)
        {
            var rest = Sum(value, weights) % 11;

            if (rest == 0)
            {
                return 0;
            }

            return (byte)(11 - rest);
        }

        private static int Sum(ReadOnlySpan<char> value, ReadOnlySpan<byte> weights)
        {
            var sum = 0;

            for (var i = 0; i < weights.Length; i++)
            {
                sum += ParsingHelpers.GetDigit(value[i]) * weights[i];
            }

            return sum;
        }
    }
}
