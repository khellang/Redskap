using BenchmarkDotNet.Attributes;
using NinEngine;
using NoCommons.Person;
using Nonin;
using TeddValidator = Tedd.Fodselsnummer.FodselsnummerValidator;

namespace Redskap.Benchmarks
{
    public class IsValidIdentificationNumberBenchmark
    {
        private const string Value = "28089647063";

        [Benchmark(Baseline = true)]
        public bool Redskap()
        {
            return IdentificationNumber.IsValid(Value);
        }

        [Benchmark]
        public bool Nonin()
        {
            return Nin.IsValid(Value);
        }

        [Benchmark]
        public bool NoCommons()
        {
            return FodselsnummerValidator.IsValid(Value);
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
