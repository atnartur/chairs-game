using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserLoggedIn : ISendableMessage
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}