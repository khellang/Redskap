# 🇳🇴 Redskap

Currently there's only one utility in the library, `IdentificationNumber`, which lets you parse, validate and generate Norwegian National Identity Numbers. More utilities are coming!

The library targets .NET Standard 2.0 and .NET 5, which means it's supported on pretty much any .NET platform still supported by Microsoft.

## Parsing

The library supports parsing fødselsnummer, D-nummer and H-nummer. This is done through the various overloads of `IdentificationNumber.Parse` and `IdentificationNumber.TryParse`.
There's also a few overloads of `IsValid` to validate that a string is a valid identification number (of a specified type).
Additionally, there are convenience methods for parsing out date of birth (using `TryParseDateOfBirth`) and gender (using `TryParseGender`) directly.

Parsing is built on modern APIs such as `ReadOnlySpan<T>`, which makes it really fast and completely allocation free. See benchmarks below.

### Benchmarks

The results of the [`IsValidIdentificationNumberBenchmark`](https://github.com/khellang/Redskap/blob/main/perf/Redskap.Benchmarks/IsValidIdentificationNumberBenchmark.cs) on .NET 10:

| Method    | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Redskap   |  16.88 ns |  0.069 ns |  0.061 ns |  1.00 |    0.00 |      - |         - |          NA |
| Nonin     |  77.54 ns |  1.115 ns |  0.989 ns |  4.59 |    0.06 | 0.0370 |     232 B |          NA |
| NoCommons | 143.06 ns |  2.581 ns |  2.288 ns |  8.48 |    0.14 | 0.0484 |     304 B |          NA |
| Tedd      | 893.48 ns | 14.738 ns | 13.786 ns | 52.90 |    0.86 | 0.3633 |    2280 B |          NA |
| NinEngine | 264.39 ns |  3.212 ns |  2.847 ns | 15.66 |    0.18 | 0.0200 |     128 B |          NA |

## Generation

The library also supports generating valid fødselsnummer, D-nummer and H-nummer.
This is done through the various overloads of `IdentificationNumber.Generate`. These let you specify kind, gender, date of birth and/or a min- and max date.
If you want to control the randomness of the generation, e.g. for unit testing, you can create your own instance of `IdentificationNumber.Generator` and pass in your own (seeded) `Random` instance. See [this unit test](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/test/Redskap.Tests/IdentificationNumberTests.cs#L43-L61) as an example.

### Benchmarks

The results of the [`GenerateIdentificationNumberBenchmark`](https://github.com/khellang/Redskap/blob/main/perf/Redskap.Benchmarks/GenerateIdentificationNumberBenchmark.cs) on .NET 10:

| Method    | Mean          | Error        | StdDev       | Ratio    | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|---------- |--------------:|-------------:|-------------:|---------:|--------:|--------:|-------:|----------:|------------:|
| Redskap   |      85.93 ns |     0.449 ns |     0.375 ns |     1.00 |    0.00 |       - |      - |         - |          NA |
| NoCommons | 630,182.62 ns | 9,721.718 ns | 8,118.075 ns | 7,334.22 |   98.93 | 85.9375 | 8.7891 |  541961 B |          NA |
| NinEngine |            NA |           NA |           NA |        ? |       ? |      NA |     NA |        NA |           ? |

## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=khellang&utm_medium=Redskap) and [Dapper Plus](https://dapper-plus.net/?utm_source=khellang&utm_medium=Redskap) are major sponsors and proud to contribute to the development of Redskap.

[![Entity Framework Extensions](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=khellang&utm_medium=Redskap)

[![Dapper Plus](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=khellang&utm_medium=Redskap)
