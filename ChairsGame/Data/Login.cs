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
            {
                global.Game = new Game();
            }
            
            await global.AddSocketAsync(webSocket, Username);
            
            global.SendMessageToAllAsync(new Message<UserLoggedIn>()
            {
                Name = "user_logged_in",
                Data = new UserLoggedIn()
                {
                    Username = Username
                }
            }, global.Game.users);

            SendCountsAndIsFirstToAll(global);

            await global.SendMessageAsync(new Message<Nothing>()
            {
                Name = "wait",
                Data = null
            }, global.Game.queue.FirstOrDefault(x=>x.Username==Username).Socket);
        }

        public static void SendCountsAndIsFirstToAll(Global global)
        {
            if (global.Game.users.Count > 0)
            {
                global.SendMessageToAllAsync(new Message<UserLoggedCount>
                {
                    Name = "user_logged_count",
                    Data = new UserLoggedCount() {Count = global.Game.users.Count}
                }, global.Game.users);

                global.SendMessageAsync(new Message<UserIsFirst>
                {
                    Name = "user_is_first",
                    Data = new UserIsFirst() {IsFirst = true}
                }, global.Game.users[0].Socket);
            }
        }
    }
}