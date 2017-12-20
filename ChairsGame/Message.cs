using Newtonsoft.Json;

namespace ChairsGame
{
    public class Message<T>
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
