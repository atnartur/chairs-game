using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChairsGame
{
    public class Global
    {
        Game Game;
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public async Task<string> AddSocketAsync(WebSocket socket, string uN)
        {
            //var uN = CreateConnectionId();
            //var uN = username;
            _sockets.TryAdd(uN, socket);

            var f = Game.users.Count == 0 ? true : false;

            Game.users.Add(new User
            {
                Username = uN,
                Socket = socket,
                First = f
            });

            return uN;
        }

        public void Parse(WebSocketReceiveResult json)
        {
            //JsonSerializer.
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out WebSocket socket);

            var f = Game.users.Count == 1 ? true : false;

            Game.users.Remove(Game.users.FirstOrDefault(x => x.Username == id));

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task SendMessageToAllAsync(Message message)
        {
            foreach (var pair in _sockets)
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        private async Task SendMessageAsync(WebSocket socket, Message mes)
        {
            var message = JsonConvert.SerializeObject(mes);

            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                    offset: 0,
                                                                    count: message.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None);
        }
        class LoginData
        {
            public string username { get; set; }
        }
        public async Task RunCommandAsync(Message message, WebSocket webSocket)
        {
            if (message.Name.ToLower() == "login")
            {
                var isFirst = false;
                if (Game == null)
                {
                    Game = new Game();
                    isFirst = true;
                }
                var username = "";
                try
                {
                    var data = ((JValue)message.Data).ToObject<LoginData>();
                    username = data.username;
                }
                catch(Exception e)
                {
                    Console.WriteLine(                      e);
                }
                await AddSocketAsync(webSocket, username);

                SendMessageToAllAsync(new Message()
                {
                    Name = "user_logged_in",
                    Data = new { username }
                });

                SendMessageToAllAsync(new Message()
                {
                    Name = "user_logged_count",
                    Data = new { count = Game.users.Count() }
                });

                SendMessageAsync(webSocket, new Message()
                {
                    Name = "user_is_first",
                    Data = new { is_first = isFirst }
                });
            }
        }
    }
}
