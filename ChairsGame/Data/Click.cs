using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class Click : ISendableMessage
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

            if (global.Game.users.Where(x => x.IsClicked == true && x.IsKicked == false).Count() == 1)
                await global.SendMessageAsync(new Message<ClickedOnChair>()
                {
                    Name = "win",
                    Data = null
                }, global.Game.users.FirstOrDefault(x => x.IsClicked == true && x.IsKicked == false).Socket);
            else
                await global.SendMessageToAllAsync(new Message<StartGameEntity>()
                {
                    Name = "startGame",
                    Data = new StartGameEntity()
                    {
                        CountOfChairs = global.Game.users.Where(x => x.IsKicked == false).Count()
                    }
                });
        }
    }
}
