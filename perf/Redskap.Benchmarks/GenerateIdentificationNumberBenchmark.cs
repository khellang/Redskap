using System;
using BenchmarkDotNet.Attributes;
using NinEngine;
using NoCommons.Person;

namespace Redskap.Benchmarks;

public class GenerateIdentificationNumberBenchmark
{
    [Benchmark(Baseline = true)]
    public IdentificationNumber Redskap()
    {
        return IdentificationNumber.Generate(IdentificationNumber.Kind.FNumber, DateTime.Now);
    }

    [Benchmark]
    public Fodselsnummer NoCommons()
    {
        return FodselsnummerCalculator.GetFodselsnummerForDate(DateTime.Now);
    }

    [Benchmark]
    public BirthNumber NinEngine()
    {
        return BirthNumber.OneRandom(DateTime.Now, DateTime.Now);
    }
}