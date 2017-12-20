using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        //private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string username)
        {
            return Game.users.FirstOrDefault(p => p.Username == username).Socket;
        }

        public List<User> GetAll()
        {
            return Game.users;
        }

        public string GetUsername(WebSocket socket)
        {
            return Game.users.FirstOrDefault(p => p.Socket == socket).Username;
        }

        public async Task<string> AddSocketAsync(WebSocket socket, string username)
        {
            //var uN = CreateConnectionId();
            //var uN = username;
            //_sockets.TryAdd(username, socket);

            var f = Game.users.Count == 0 ? true : false;

            if (Game.users.FirstOrDefault(x => x.Username == username) == null)
                Game.users.Add(new User
                {
                    Username = username,
                    Socket = socket,
                    IsFirst = f
                });

            return username;
        }

        public async Task RemoveSocket(string id)
        {
            //_sockets.TryRemove(id, out WebSocket socket);
            var user = Game.users.FirstOrDefault(x => x.Username == id);

            var socket = user.Socket;

            var f = Game.users.Count == 1 ? true : false;

            Game.users.Remove(user);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageToAllAsync<T>(Message<T> message) where T : ISendableMessage
        {
            foreach (var socket in Game.users)
            {
                if (socket.Socket.State == WebSocketState.Open)
                    await SendMessageAsync<T>(message, socket.Socket);
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
            else if (decodedForName.Name == "startGame")
                await Run<StartGame>(message, webSocket);
            else if (decodedForName.Name == "click")
                await Run<Click>(message, webSocket);
        }
    }
}
