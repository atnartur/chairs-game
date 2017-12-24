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
            }, global.Game.users);

            global.Game.users.FirstOrDefault(x => x.Socket == webSocket).IsClicked = true;

            if (global.Game.users.Where(x => x.IsClicked == false && x.IsKicked == false).Count() == 1)
            {
                var user = global.Game.users.FirstOrDefault(x => x.IsClicked == false && x.IsKicked == false);
                await global.SendMessageAsync(new Message<Nothing>()
                {
                    Name = "kick",
                    Data = null
                }, user.Socket);
                user.IsKicked = true;
            }

            if (global.Game.users.Count(x => x.IsClicked && !x.IsKicked) == 1)
            {
                await global.SendMessageAsync(new Message<Nothing>()
                {
                    Name = "win",
                    Data = null
                }, global.Game.users.FirstOrDefault(x => x.IsClicked && !x.IsKicked).Socket);



                global.Game.users.ForEach(x => global.RemoveSocket(x.Username));
                global.Game.users.Clear();
                global.Game.queue.ForEach(x => global.Game.users.Add(x));
                global.Game.queue.Clear();

                global.Game.users.ForEach(x =>
                {
                    global.SendMessageToAllAsync(new Message<UserLoggedIn>()
                    {
                        Name = "user_logged_in",
                        Data = new UserLoggedIn()
                        {
                            Username = x.Username
                        }
                    }, global.Game.users);
                });

                SendCountsAndIsFirstToAll(global);
            }
            else
                await global.SendMessageToAllAsync(new Message<StartGameEntity>()
                {
                    Name = "startGame",
                    Data = new StartGameEntity()
                    {
                        CountOfChairs = global.Game.users.Count(x => !x.IsKicked)
                    }
                }, global.Game.users);
        }

        public static void SendCountsAndIsFirstToAll(Global global)
        {
            if (global.Game.users.Count > 0)
            {
                global.SendMessageToAllAsync(new Message<UserLoggedCount>
                {
                    Name = "user_logged_count",
                    Data = new UserLoggedCount() { Count = global.Game.users.Count }
                }, global.Game.users);

                global.SendMessageAsync(new Message<UserIsFirst>
                {
                    Name = "user_is_first",
                    Data = new UserIsFirst() { IsFirst = true }
                }, global.Game.users[0].Socket);
            }
        }
    }
}
