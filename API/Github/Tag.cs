using System.Text.Json.Serialization;

namespace Alpalis.UtilityServices.API.Github
{
    public class Tag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("zipball_url")]
        public string ZipballUrl { get; set; } = null!;

        [JsonPropertyName("tarball_url")]
        public string TarballUrl { get; set; } = null!;

        [JsonPropertyName("commit")]
        public Commit Commit { get; set; } = null!;

        [JsonPropertyName("node_id")]
        public string NodeId { get; set; } = null!;
    }
}
