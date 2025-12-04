```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
Intel Core i5-9400F CPU 2.90GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET SDK 9.0.304 [C:\Program Files\dotnet\sdk]
  [Host] : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method                 | Job       | Toolchain              | IterationCount | LaunchCount | RunStrategy | UnrollFactor | WarmupCount | Mean     | Error    | StdDev   |
|----------------------- |---------- |----------------------- |--------------- |------------ |------------ |------------- |------------ |---------:|---------:|---------:|
| ComputeWithInterlocked | Dry       | Default                | 1              | 1           | ColdStart   | 1            | 1           |       NA |       NA |       NA |
| ComputeWithLock        | Dry       | Default                | 1              | 1           | ColdStart   | 1            | 1           |       NA |       NA |       NA |
| ComputeWithInterlocked | InProcess | InProcessEmitToolchain | Default        | Default     | Default     | 16           | Default     | 23.09 ms | 0.063 ms | 0.059 ms |
| ComputeWithLock        | InProcess | InProcessEmitToolchain | Default        | Default     | Default     | 16           | Default     | 84.63 ms | 0.280 ms | 0.262 ms |

Benchmarks with issues:
  Workload.ComputeWithInterlocked: Dry(IterationCount=1, LaunchCount=1, RunStrategy=ColdStart, UnrollFactor=1, WarmupCount=1)
  Workload.ComputeWithLock: Dry(IterationCount=1, LaunchCount=1, RunStrategy=ColdStart, UnrollFactor=1, WarmupCount=1)
