﻿using Newtonsoft.Json;

namespace DNet.Structures.Channels
{
    public struct PermissionOverwrite
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("allow")]
        public int Allow { get; set; }

        [JsonProperty("deny")]
        public int Deny { get; set; }
    }
}
