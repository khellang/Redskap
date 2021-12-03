using System;
using System.Runtime.CompilerServices;

namespace Redskap
{
    internal static class ParsingHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte GetDigit(char c)
        {
            return (byte) (c - '0');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetDigit(char c, out byte digit)
        {
            digit = GetDigit(c);
            return digit is >= 0 and <= 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryReadTwoDecimalDigits(ReadOnlySpan<char> source, int offset, out int digit)
        {
            if (TryGetDigit(source[offset + 1], out var first) &&
                TryGetDigit(source[offset + 0], out var second))
            {
                digit = first + (second * 10);
                return true;
            }

            digit = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryReadThreeDecimalDigits(ReadOnlySpan<char> source, int offset, out int digit)
        {
            if (TryGetDigit(source[offset + 2], out var a) &&
                TryGetDigit(source[offset + 1], out var b) &&
                TryGetDigit(source[offset + 0], out var c))
            {
                digit = a + (b * 10) + (c * 100);
                return true;
            }

            digit = default;
            return false;
        }
    }
}
