using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class StartGameEntity : ISendableMessage
    {
        [JsonProperty("countOfChairs")]
        public int CountOfChairs { get; set; }
    }
}
