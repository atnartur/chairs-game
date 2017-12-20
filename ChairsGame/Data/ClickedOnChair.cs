using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class ClickedOnChair : ISendableMessage
    {
        [JsonProperty("numberOfChair")]
        public string NumberOfChair { get; set; }
    }
}
