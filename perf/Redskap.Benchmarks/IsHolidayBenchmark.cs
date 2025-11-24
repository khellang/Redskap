using System;
using BenchmarkDotNet.Attributes;
using Nager.Date;
using NoCommons.Date;

namespace Redskap.Benchmarks;

public class IsHolidayBenchmark
{
    private static readonly DateTime Date = new(2021, 04, 01);

    static IsHolidayBenchmark()
    {
        HolidaySystem.LicenseKey = "LostTimeIsNeverFoundAgain";
    }

    [Benchmark(Baseline = true)]
    public bool Redskap()
    {
        return Date.IsHoliday();
    }

    [Benchmark]
    public bool NoCommons()
    {
        return NorwegianDateUtil.IsHoliday(Date);
    }

    [Benchmark]
    public bool Nager()
    {
        return HolidaySystem.IsPublicHoliday(Date, CountryCode.NO);
    }
}
