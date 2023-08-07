using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Redskap;

internal static class FormattingHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static char GetChar(uint digit) => (char) ('0' + digit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void WriteDigit(uint value, Span<char> destination, int offset)
    {
        Debug.Assert(value <= 9);

        destination[offset] = GetChar(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void WriteTwoDecimalDigits(uint value, Span<char> destination, int offset)
    {
        Debug.Assert(value <= 99);

        var temp = value;
        value /= 10;
        destination[offset + 1] = GetChar(temp - (value * 10));

        destination[offset] = GetChar(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteThreeDecimalDigits(uint value, Span<char> destination, int offset)
    {
        Debug.Assert(value <= 999);

        var temp = value;
        value /= 10;
        destination[offset + 2] = GetChar(temp - (value * 10));

        temp = value;
        value /= 10;
        destination[offset + 1] = GetChar(temp - (value * 10));

        destination[offset] = GetChar(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFourDecimalDigits(uint value, Span<char> destination, int offset)
    {
        Debug.Assert(value <= 9999);

        var temp = value;
        value /= 10;
        destination[offset + 3] = GetChar(temp - (value * 10));

        temp = value;
        value /= 10;
        destination[offset + 2] = GetChar(temp - (value * 10));

        temp = value;
        value /= 10;
        destination[offset + 1] = GetChar(temp - (value * 10));

        destination[offset] = GetChar(value);
    }
}
