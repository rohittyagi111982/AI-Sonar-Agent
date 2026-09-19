using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCPServerAPI.Model
{
    public class SonarComponentResponse
    {
        [JsonPropertyName("component")]
        public SonarComponent Component { get; set; } = new();
    }

    public class SonarComponent
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("qualifier")]
        public string Qualifier { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }
}
