using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserLoggedCount : ISendableMessage
    {
        public UserLoggedCount(int count)
        {
            Count = count;
        }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}