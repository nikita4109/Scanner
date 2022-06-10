using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Entities
{
    /// <summary>
    /// Stores data on suspicious.
    /// </summary>
    [Serializable]
    public class Suspicious
    {
        [JsonPropertyName("Description")] public string Description { get; set; }
        [JsonPropertyName("BadString")] public string BadString { get; set; }
        [JsonPropertyName("FilesCount")] public int FilesCount { get; set; }
        [JsonPropertyName("Extensions")] public List<string> Extensions { get; set; }

        public Suspicious(string description, string badString, int filesCount, List<string> extensions)
        {
            Description = description;
            BadString = badString;
            FilesCount = filesCount;
            Extensions = extensions;
        }
    }
}