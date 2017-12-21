using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class ParameterlessMessage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
