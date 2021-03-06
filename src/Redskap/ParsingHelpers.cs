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
        internal static int ReadTwoDecimalDigits(ReadOnlySpan<char> source, int offset)
        {
            return GetDigit(source[offset + 1])
                + (GetDigit(source[offset + 0]) * 10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ReadThreeDecimalDigits(ReadOnlySpan<char> source, int offset)
        {
            return GetDigit(source[offset + 2])
                + (GetDigit(source[offset + 1]) * 10)
                + (GetDigit(source[offset + 0]) * 100);
        }
    }
}
