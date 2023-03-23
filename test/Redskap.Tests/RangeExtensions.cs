using System;
using System.Collections.Generic;

namespace Redskap.Tests;

internal static class RangeExtensions
{
    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            throw new ArgumentException(nameof(range));
        }

        for (var i = range.Start.Value; i <= range.End.Value; i++)
        {
            yield return i;
        }
    }
}