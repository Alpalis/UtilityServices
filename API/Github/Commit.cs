using System.Text.Json.Serialization;

namespace Alpalis.UtilityServices.API.Github
{
    public class Commit
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; } = null!;

        [JsonPropertyName("url")]
        public string Url { get; set; } = null!;
    }
}
