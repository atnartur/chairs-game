using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class UserIsFirst : ISendableMessage
    {
        public UserIsFirst(bool isFirst)
        {
            IsFirst = isFirst;
        }

        [JsonProperty("is_first")]
        public bool IsFirst { get; set; }
    }
}