using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities
{
    /// <summary>
    /// Report stores all information about the result of a directory scan.
    /// </summary>
    [Serializable]
    public class Report
    {
        [JsonPropertyName("Directory")] public string Directory { get; set; }
        [JsonPropertyName("FilesCount")] public int FilesCount { get; set; }
        [JsonPropertyName("Suspicious")] public List<Suspicious> Suspicious { get; set; }
        [JsonPropertyName("ErrorsCount")] public int ErrorsCount { get; set; }
        [JsonPropertyName("ExecutionTime")] public string ExecutionTime { get; set; }

        public Report(string directory, int filesCount, List<Suspicious> suspicious, int errorsCount,
            string executionTime)
        {
            Directory = directory;
            FilesCount = filesCount;
            Suspicious = suspicious;
            ErrorsCount = errorsCount;
            ExecutionTime = executionTime;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder($"====== Scan result ======\n" +
                                                     $"Directory: {Directory}\n" +
                                                     $"Processed files: {FilesCount}\n");

            foreach (var suspicious in Suspicious)
            {
                result.Append($"{suspicious.Description}: {suspicious.FilesCount}\n");
            }

            result.Append($"Errors: {ErrorsCount}\n" +
                          $"Execution time: {ExecutionTime}\n" +
                          $"=========================");

            return result.ToString();
        }
    }
}