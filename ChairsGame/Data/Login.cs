using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChairsGame.Data
{
    public class Login : IRecievedMessage
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        public async Task Run(Global global, WebSocket webSocket)
        {
            if (global.Game == null)
                global.Game = new Game();

            global.AddSocketAsync(webSocket, Username);

            await global.SendMessageToAllAsync(new Message<UserLoggedIn>()
            {
                Name = "user_logged_in",
                Data = new UserLoggedIn(Username)
            }, global.Game.users);

            await SendCountsAndIsFirstToAll(global);

            if (global.Game.IsStart)
                await global.SendMessageAsync(new Message<Nothing>()
                {
                    Name = "wait",
                    Data = null
                }, global.Game.queue.FirstOrDefault(x => x.Username == Username).Socket);
        }

        public static async Task SendCountsAndIsFirstToAll(Global global)
        {
            if (global.Game.users.Count > 0)
            {
                await global.SendMessageToAllAsync(new Message<UserLoggedCount>
                {
                    Name = "user_logged_count",
                    Data = new UserLoggedCount(global.Game.users.Count)
                }, global.Game.users);

                await global.SendMessageAsync(new Message<UserIsFirst>
                {
                    Name = "user_is_first",
                    Data = new UserIsFirst(true)
                }, global.Game.users[0].Socket);
            }
        }
    }
}