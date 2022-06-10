using System;
using System.Text.Json.Serialization;

namespace Entities
{
    [Serializable]
    public class Entity
    {
        [JsonPropertyName("Status")] public Status Status { get; set; }
        [JsonPropertyName("Report")] public Report Report { get; set; }

        public Entity(Status status, Report report)
        {
            Status = status;
            Report = report;
        }
    }
}