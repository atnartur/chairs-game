using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChairsGame.Data;
using Newtonsoft.Json;

namespace ChairsGame
{
    public class Global
    {
        public Game Game { get; set; }
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

        public async Task<string> AddSocketAsync(WebSocket socket, string username)
        {
            //var uN = CreateConnectionId();
            //var uN = username;
            _sockets.TryAdd(username, socket);

            var f = Game.users.Count == 0 ? true : false;

            Game.users.Add(new User
            {
                Username = username,
                Socket = socket,
                First = f
            });

            return username;
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

        public async Task SendMessageToAllAsync<T>(Message<T> message) where T : ISendableMessage
        {
            foreach (var pair in _sockets)
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync<T>(message, pair.Value);
            }
        }

        public async Task SendMessageAsync<T>(Message<T> mes, WebSocket socket) where T : ISendableMessage
        {
            var message = JsonConvert.SerializeObject(mes);

            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(
                buffer: new ArraySegment<byte>(
                    array: Encoding.ASCII.GetBytes(message),
                    offset: 0,
                    count: message.Length
                ),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None
            );
        }

        private async Task Run<T>(string message, WebSocket webSocket) where T : IRecievedMessage =>
            await JsonConvert.DeserializeObject<Message<T>>(message).Data.Run(this, webSocket);
        
        public async Task RunCommandAsync(string message, WebSocket webSocket)
        {
            var decodedForName = JsonConvert.DeserializeObject<Message<object>>(message);
            if (decodedForName.Name == "login")
                await Run<Login>(message, webSocket);
        }
    }
}
