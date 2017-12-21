using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class Click : ISendableMessage, IRecievedMessage
    {
        [JsonProperty("numberOfChair")]
        public string NumberOfChair { get; set; }
        public async Task Run(Global global, WebSocket webSocket)
        {
            await global.SendMessageToAllAsync(new Message<ClickedOnChair>()
            {
                Name = "clickedOnChair",
                Data = new ClickedOnChair()
                {
                    NumberOfChair = NumberOfChair
                }
            });

            global.Game.users.FirstOrDefault(x => x.Socket == webSocket).IsClicked = true;

            if (global.Game.users.Where(x => x.IsClicked == false && x.IsKicked == false).Count() == 1)
            {
                var user = global.Game.users.FirstOrDefault(x => x.IsClicked == false && x.IsKicked == false);
                await global.SendMessageAsync(new Message<ClickedOnChair>()
                {
                    Name = "kick",
                    Data = null
                }, user.Socket);
                user.IsKicked = true;
            }

            if (global.Game.users.Count(x => x.IsClicked && !x.IsKicked) == 1)
                await global.SendMessageAsync(new Message<ClickedOnChair>()
                {
                    Name = "win",
                    Data = null
                }, global.Game.users.FirstOrDefault(x => x.IsClicked && !x.IsKicked).Socket);
            else
                await global.SendMessageToAllAsync(new Message<StartGameEntity>()
                {
                    Name = "startGame",
                    Data = new StartGameEntity()
                    {
                        CountOfChairs = global.Game.users.Count(x => !x.IsKicked)
                    }
                });
        }
    }
}
