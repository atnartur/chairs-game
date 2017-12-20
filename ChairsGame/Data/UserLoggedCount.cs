using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserLoggedCount : ISendableMessage
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}