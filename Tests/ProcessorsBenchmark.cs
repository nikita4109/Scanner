using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Entities;
using NUnit.Framework;
using Server.Models;

namespace Tests
{
    public class ProcessorsBenchmark
    {
        [Test]
        public void StartBenchmarks()
        {
            BenchmarkRunner.Run<ProcessorsBenchmark>();
        }

        [Benchmark(Description = "Naive")]
        public void NaiveSearchThreeStrings()
        {
            var text = GenerateString().ReadToEnd();
            bool js = text.Contains(@"<script>evil_script()</script>");
            bool rm = text.Contains(@"rm -rf %userprofile%\Documents");
            bool dll = text.Contains(@"Rundll32 sus.dll SusEntry");
        }

        [Benchmark(Description = "Aho")]
        public void AhoCorasickSearchThreeStrings()
        {
            var aho = new AhoCorasick();
            Suspicious js = new Suspicious(
                "JS",
                @"<script>evil_script()</script>",
                0,
                new List<string>() {".js"});

            Suspicious rm = new Suspicious(
                "rm -rf",
                @"rm -rf %userprofile%\Documents",
                0,
                new List<string>());

            Suspicious dll = new Suspicious(
                "Rundll32",
                @"Rundll32 sus.dll SusEntry",
                0,
                new List<string>());

            aho.Add(js);
            aho.Add(rm);
            aho.Add(dll);

            aho.Process(GenerateString());
        }

        private StreamReader GenerateString()
        {
            Random random = new Random(42);

            StringBuilder builder = new StringBuilder();

            int n = 5000000;

            int length = random.Next(n, 2 * n);
            for (int i = 0; i < length; ++i)
            {
                builder.Append(random.Next(Char.MaxValue));
            }

            var suspicious = new List<string>()
            {
                @"<script>evil_script()</script>",
                @"rm -rf %userprofile%\Documents",
                @"Rundll32 sus.dll SusEntry"
            };

            builder.Append(suspicious[random.Next(3)]);

            length = random.Next(n, 2 * n);
            for (int i = 0; i < length; ++i)
            {
                builder.Append(random.Next(Char.MaxValue));
            }

            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString())));
        }
    }
}