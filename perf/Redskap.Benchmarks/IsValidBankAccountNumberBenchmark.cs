using BenchmarkDotNet.Attributes;
using NoCommons.Banking;

namespace Redskap.Benchmarks;

public class IsValidBankAccountNumberBenchmark
{
    private const string Value = "93398290133";

    [Benchmark(Baseline = true)]
    public bool Redskap()
    {
        return BankAccountNumber.IsValid(Value);
    }

    [Benchmark]
    public bool NoCommons()
    {
        return KontonummerValidator.IsValid(Value);
    }
}