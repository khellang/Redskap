using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Nonin;
using NinEngine;
using TeddValidator = Tedd.Fodselsnummer.FodselsnummerValidator;
using NoCommonsValidator = NoCommons.Person.FodselsnummerValidator;

namespace Redskap.Benchmarks
{
    [MemoryDiagnoser]
    public class Program
    {
        private const string Value = "28089647063";

        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                // NoCommons and NinEngine ships with unoptimized assemblies
                .WithOptions(ConfigOptions.DisableOptimizationsValidator);

            BenchmarkRunner.Run<Program>(config);
        }

        [Benchmark(Baseline = true)]
        public bool Lothbrok()
        {
            return IdentificationNumber.TryParse(Value, out _);
        }

        [Benchmark]
        public bool Nonin()
        {
            return Nin.IsValid(Value);
        }

        [Benchmark]
        public bool NoCommons()
        {
            return NoCommonsValidator.IsValid(Value);
        }

        [Benchmark]
        public bool Tedd()
        {
            return TeddValidator.Validate(Value).Success;
        }

        [Benchmark]
        public bool NinEngine()
        {
            return NorwegianIdentity.IsValidFoedselsnummer(Value);
        }
    }
}
