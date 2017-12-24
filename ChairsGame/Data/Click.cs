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
            var currentUser = global.Game.users.FirstOrDefault(x => x.Socket == webSocket);

            if (!currentUser.IsClicked)
            {
                await global.SendMessageToAllAsync(new Message<ClickedOnChair>()
                {
                    Name = "clickedOnChair",
                    Data = new ClickedOnChair()
                    {
                        NumberOfChair = NumberOfChair
                    }
                }, global.Game.users);

                currentUser.IsClicked = true;

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

                    await global.RemoveUsers(global.Game.users.Select(x => x.Username).ToArray());
                    global.Game.users.Clear();
                    global.Game.IsStart = false;
                    global.Game.queue.ForEach(x => global.Game.users.Add(x));
                    global.Game.queue.Clear();

                    await global.Game.users.ForEachAsync(async x =>
                    {
                        await global.SendMessageToAllAsync(new Message<UserLoggedIn>()
                        {
                            Name = "user_logged_in",
                            Data = new UserLoggedIn(x.Username)
                        }, global.Game.users);
                    });

                    await Login.SendCountsAndIsFirstToAll(global);
                }
                else
                    await StartGame.Send(global);
            }
        }
    }
}
