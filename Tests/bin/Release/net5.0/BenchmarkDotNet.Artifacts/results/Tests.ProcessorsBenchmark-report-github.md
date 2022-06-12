``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.102
  [Host]     : .NET 5.0.2 (5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET 5.0.2 (5.0.220.61120), X64 RyuJIT


```
| Method |       Mean |    Error |    StdDev |
|------- |-----------:|---------:|----------:|
|  Naive |   614.2 ms | 14.00 ms |  40.62 ms |
|    Aho | 1,275.5 ms | 35.83 ms | 101.64 ms |
