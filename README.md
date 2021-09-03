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

| Method    | Runtime              |        Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
| --------- | -------------------- | ----------: | --------: | --------: | ----: | ------: | -----: | --------: |
| Redskap   | .NET 5.0             |    58.23 ns |  0.478 ns |  0.423 ns |  0.60 |    0.01 |      - |         - |
| Nonin     | .NET 5.0             |   270.45 ns |  3.257 ns |  2.888 ns |  2.79 |    0.04 | 0.0315 |     264 B |
| NoCommons | .NET 5.0             |   923.89 ns |  5.672 ns |  5.306 ns |  9.52 |    0.07 | 0.0458 |     384 B |
| Tedd      | .NET 5.0             | 2,354.07 ns | 23.517 ns | 21.998 ns | 24.29 |    0.30 | 0.2975 |   2,512 B |
| NinEngine | .NET 5.0             |   383.86 ns |  3.804 ns |  3.558 ns |  3.96 |    0.04 | 0.0153 |     128 B |
| Redskap   | .NET Framework 4.7.2 |    96.97 ns |  0.594 ns |  0.527 ns |  1.00 |    0.00 |      - |         - |
| Nonin     | .NET Framework 4.7.2 |   480.74 ns |  7.024 ns |  6.570 ns |  4.95 |    0.06 | 0.0410 |     265 B |
| NoCommons | .NET Framework 4.7.2 | 1,348.68 ns |  5.854 ns |  5.475 ns | 13.91 |    0.07 | 0.0629 |     409 B |
| Tedd      | .NET Framework 4.7.2 | 4,086.77 ns | 33.722 ns | 31.544 ns | 42.13 |    0.49 | 0.4349 |   2,760 B |
| NinEngine | .NET Framework 4.7.2 |   648.94 ns |  5.967 ns |  5.289 ns |  6.69 |    0.06 | 0.0200 |     128 B |

## Generation

The library also supports generating valid fødselsnummer, D-nummer and H-nummer.
This is done through the various overloads of `IdentificationNumber.Generate`. These let you specify kind, gender, date of birth and/or a min- and max date.
If you want to control the randomness of the generation, e.g. for unit testing, you can create your own instance of `IdentificationNumber.Generator` and pass in your own (seeded) `Random` instance. See [this unit test](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/test/Redskap.Tests/IdentificationNumberTests.cs#L43-L61) as an example.

### Benchmarks

The results of the [`GenerateBenchmark`](https://github.com/khellang/Redskap/blob/b2b6ae87542825d379793ef6c8b1508012786616/perf/Redskap.Benchmarks/GenerateBenchmark.cs):

| Method    | Runtime              |           Mean |        Error |       StdDev |    Ratio | RatioSD |    Gen 0 |   Gen 1 | Allocated |
| --------- | -------------------- | -------------: | -----------: | -----------: | -------: | ------: | -------: | ------: | --------: |
| Redskap   | .NET 5.0             |       328.0 ns |      2.95 ns |      2.76 ns |     0.82 |    0.01 |        - |       - |         - |
| NoCommons | .NET 5.0             | 2,173,367.9 ns | 11,802.91 ns | 10,462.97 ns | 5,411.11 |   37.73 |  74.2188 |  7.8125 | 621,440 B |
| NinEngine | .NET 5.0             |     2,335.9 ns |     14.71 ns |     13.04 ns |     5.82 |    0.03 |   0.3662 |       - |   3,090 B |
| Redskap   | .NET Framework 4.7.2 |       401.8 ns |      1.76 ns |      1.65 ns |     1.00 |    0.00 |        - |       - |         - |
| NoCommons | .NET Framework 4.7.2 | 2,734,492.1 ns | 11,812.94 ns | 11,049.83 ns | 6,805.64 |   34.93 | 117.1875 | 11.7188 | 745,792 B |
| NinEngine | .NET Framework 4.7.2 |     2,920.9 ns |      8.30 ns |      7.36 ns |     7.27 |    0.03 |   0.5722 |       - |   3,623 B |
