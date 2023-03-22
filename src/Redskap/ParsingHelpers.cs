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
        internal static bool TryReadTwoDecimalDigits(ReadOnlySpan<char> source, int offset, out int number)
        {
            if (TryGetDigit(source[offset + 1], out var first) &&
                TryGetDigit(source[offset + 0], out var second))
            {
                number = first + (second * 10);
                return true;
            }

            number = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryReadThreeDecimalDigits(ReadOnlySpan<char> source, int offset, out int number)
        {
            if (TryGetDigit(source[offset + 2], out var a) &&
                TryGetDigit(source[offset + 1], out var b) &&
                TryGetDigit(source[offset + 0], out var c))
            {
                number = a + (b * 10) + (c * 100);
                return true;
            }

            number = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryReadFourDecimalDigits(ReadOnlySpan<char> source, int offset, out int number)
        {
            if (TryGetDigit(source[offset + 3], out var a) &&
                TryGetDigit(source[offset + 2], out var b) &&
                TryGetDigit(source[offset + 1], out var c) &&
                TryGetDigit(source[offset + 0], out var d))
            {
                number = a + (b * 10) + (c * 100) + (d * 1000);
                return true;
            }

            number = default;
            return false;
        }
    }
}
