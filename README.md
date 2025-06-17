# 🇳🇴 Redskap

Currently there's only one utility in the library, `IdentificationNumber`, which lets you parse, validate and generate Norwegian National Identity Numbers. More utilities are coming!

The library targets .NET Standard 2.0 and .NET 5, which means it's supported on pretty much any .NET platform still supported by Microsoft.

## Parsing

The library supports parsing fødselsnummer, D-nummer and H-nummer. This is done through the various overloads of `IdentificationNumber.Parse` and `IdentificationNumber.TryParse`.
There's also a few overloads of `IsValid` to validate that a string is a valid identification number (of a specified type).
Additionally, there are convenience methods for parsing out date of birth (using `TryParseDateOfBirth`) and gender (using `TryParseGender`) directly.

Parsing is built on modern APIs such as `ReadOnlySpan<T>`, which makes it really fast and completely allocation free. See benchmarks below.

### Benchmarks

The results of the [`IsValidIdentificationNumberBenchmark`](https://github.com/khellang/Redskap/blob/main/perf/Redskap.Benchmarks/IsValidIdentificationNumberBenchmark.cs) on .NET 8:

| Method    | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Redskap   |    27.63 ns |  0.291 ns |  0.272 ns |  1.00 |    0.00 |      - |         - |          NA |
| Nonin     |   118.67 ns |  1.315 ns |  1.098 ns |  4.30 |    0.06 | 0.0315 |     264 B |          NA |
| NoCommons |   265.39 ns |  2.474 ns |  2.314 ns |  9.61 |    0.15 | 0.0458 |     384 B |          NA |
| Tedd      | 1,357.05 ns | 11.306 ns | 10.023 ns | 49.10 |    0.57 | 0.2995 |    2512 B |          NA |
| NinEngine |   329.70 ns |  1.598 ns |  1.495 ns | 11.93 |    0.11 | 0.0153 |     128 B |          NA |

## Generation

The library also supports generating valid fødselsnummer, D-nummer and H-nummer.
This is done through the various overloads of `IdentificationNumber.Generate`. These let you specify kind, gender, date of birth and/or a min- and max date.
If you want to control the randomness of the generation, e.g. for unit testing, you can create your own instance of `IdentificationNumber.Generator` and pass in your own (seeded) `Random` instance. See [this unit test](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/test/Redskap.Tests/IdentificationNumberTests.cs#L43-L61) as an example.

### Benchmarks

The results of the [`GenerateIdentificationNumberBenchmark`](https://github.com/khellang/Redskap/blob/main/perf/Redskap.Benchmarks/GenerateIdentificationNumberBenchmark.cs) on .NET 8:

| Method    | Mean           | Error        | StdDev       | Ratio    | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|---------- |---------------:|-------------:|-------------:|---------:|--------:|--------:|-------:|----------:|------------:|
| Redskap   |       140.6 ns |      1.02 ns |      0.85 ns |     1.00 |    0.00 |       - |      - |         - |          NA |
| NoCommons | 1,161,481.9 ns | 12,681.80 ns | 11,862.56 ns | 8,275.50 |   80.83 | 66.4063 | 7.8125 |  566769 B |          NA |
| NinEngine |     1,632.0 ns |     28.36 ns |     26.53 ns |    11.58 |    0.19 |  0.3681 |      - |    3092 B |          NA |

## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=khellang&utm_medium=Redskap) and [Dapper Plus](https://dapper-plus.net/?utm_source=khellang&utm_medium=Redskap) are major sponsors and proud to contribute to the development of Redskap.

[![Entity Framework Extensions](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=khellang&utm_medium=Redskap)

[![Dapper Plus](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=khellang&utm_medium=Redskap)
