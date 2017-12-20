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
            var isFirst = false;
            if (global.Game == null)
            {
                global.Game = new Game();
                isFirst = true;
            }
            
            await global.AddSocketAsync(webSocket, Username);

            global.SendMessageToAllAsync(new Message<UserLoggedIn>()
            {
                Name = "user_logged_in",
                Data = new UserLoggedIn()
                {
                    Username = Username
                }
            });

            global.SendMessageToAllAsync(new Message<UserLoggedCount>()
            {
                Name = "user_logged_count",
                Data = new UserLoggedCount() { Count = global.Game.users.Count }
            });

            global.SendMessageAsync(new Message<UserIsFirst>()
            {
                Name = "user_is_first",
                Data = new UserIsFirst(){ IsFirst = isFirst }
            }, webSocket);
        }
    }
}