# 🇳🇴 Redskap

Currently there's only one utility in the library, `IdentificationNumber`, which lets you parse, validate and generate Norwegian National Identity Numbers. More utilities are coming!

The library targets .NET Standard 2.0 and .NET 5, which means it's supported on pretty much any .NET platform still supported by Microsoft.

## Parsing

The library supports parsing fødselsnummer, D-nummer and H-nummer. This is done through the various overloads of `IdentificationNumber.Parse` and `IdentificationNumber.TryParse`.  
There's also a few overloads of `IsValid` to validate that a string is a valid identification number (of a specified type).  
Additionally, there are convenience methods for parsing out date of birth (using `TryParseDateOfBirth`) and gender (using `TryParseGender`) directly.  

Parsing is built on modern APIs such as `ReadOnlySpan<T>`, which makes it really fast and completely allocation free. See benchmarks below.

### Benchmarks

The results of the [`IsValidBenchmark`](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/perf/Redskap.Benchmarks/IsValidBenchmark.cs):

|    Method |        Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |------------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
|   Redskap |    75.96 ns |  0.529 ns |  0.495 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|     Nonin |   314.09 ns |  1.386 ns |  1.296 ns |  4.14 |    0.03 | 0.0839 |     - |     - |     264 B |
| NoCommons | 1,057.82 ns |  5.434 ns |  4.538 ns | 13.94 |    0.08 | 0.1221 |     - |     - |     384 B |
|      Tedd | 2,714.70 ns | 51.943 ns | 59.818 ns | 36.00 |    0.65 | 0.7973 |     - |     - |    2512 B |
| NinEngine |   514.99 ns | 10.130 ns | 12.059 ns |  6.80 |    0.20 | 0.0401 |     - |     - |     128 B |

## Generation

The library also supports generating valid fødselsnummer, D-nummer and H-nummer.
This is done through the various overloads of `IdentificationNumber.Generate`. These let you specify kind, gender, date of birth and/or a min- and max date.  
If you want to control the randomness of the generation, e.g. for unit testing, you can create your own instance of `IdentificationNumber.Generator` and pass in your own (seeded) `Random` instance. See [this unit test](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/test/Redskap.Tests/IdentificationNumberTests.cs#L43-L61) as an example.

### Benchmarks

The results of the [`GenerateBenchmark`](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/perf/Redskap.Benchmarks/GenerateBenchmark.cs):

|    Method |           Mean |        Error |       StdDev |    Ratio | RatioSD |    Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |---------------:|-------------:|-------------:|---------:|--------:|---------:|------:|------:|----------:|
|   Redskap |       410.6 ns |      1.41 ns |      1.25 ns |     1.00 |    0.00 |   0.0100 |     - |     - |      32 B |
| NoCommons | 4,038,315.0 ns | 41,735.70 ns | 39,039.60 ns | 9,833.87 |   99.19 | 195.3125 |     - |     - |  622210 B |
| NinEngine |     2,722.1 ns |     54.19 ns |     60.23 ns |     6.67 |    0.15 |   0.9766 |     - |     - |    3075 B |
