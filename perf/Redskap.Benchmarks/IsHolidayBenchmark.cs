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
        HolidaySystem.LicenseKey = "02207B22437265617465644174223A22323032342D30332D30375430303A30303A30302B30313A3030222C22497373756564546F223A224769744875622053706F6E736F72222C2256616C6964556E74696C223A22323132342D30322D31325430303A30303A30302B30313A3030227D45E4C77D87F3C02349AD88822D8455DF1A684B1F7892B7FE130E2AFC0B6E131850E9961C8D931F56BD825E17871DC6AE1EB8B76160F151A6A59CB4FB7C0A557B";
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
