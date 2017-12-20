using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserIsFirst : ISendableMessage
    {
        [JsonProperty("is_first")]
        public bool IsFirst { get; set; }
    }
}