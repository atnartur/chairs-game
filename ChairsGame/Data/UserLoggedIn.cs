using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserLoggedIn : ISendableMessage
    {
        public UserLoggedIn(string username)
        {
            Username = username;
        }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}