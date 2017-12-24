using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class StartGameEntity : ISendableMessage
    {
        public StartGameEntity(int countOfChairs)
        {
            CountOfChairs = countOfChairs;
        }

        [JsonProperty("countOfChairs")]
        public int CountOfChairs { get; set; }
    }
}
