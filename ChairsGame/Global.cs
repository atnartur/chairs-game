using System;
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
            if (!Game.IsStart)
            {
                var f = Game.users.Count == 0 ? true : false;

                if (Game.users.FirstOrDefault(x => x.Username == username) == null)
                    Game.users.Add(new User
                    {
                        Username = username,
                        Socket = socket,
                        IsFirst = f
                    });
            }
            else
            {
                var f = Game.queue.Count == 0 ? true : false;

                if (Game.queue.FirstOrDefault(x => x.Username == username) == null)
                    Game.queue.Add(new User
                    {
                        Username = username,
                        Socket = socket,
                        IsFirst = f
                    });
            }

            return username;
        }

        public async Task CloseSocket(WebSocket socket)
        {
            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the WebSocketManager",
                cancellationToken: CancellationToken.None);
        }

        public async Task RemoveUsers(string[] usernames)
        {
            foreach (var username in usernames)
            {
                var user = Game.users.FirstOrDefault(x => x.Username == username);
                var socket = user.Socket;
                await CloseSocket(socket);
            }
            foreach (var username in usernames)
            {
                var user = Game.users.FirstOrDefault(x => x.Username == username);
                Game.users.Remove(user);
            }
            await Login.SendCountsAndIsFirstToAll(this);
        }
        
        public async Task RemoveUser(string username)
        {
            var user = Game.users.FirstOrDefault(x => x.Username == username);

            var socket = user.Socket;
//            Game.users.Reverse();
            Game.users.Remove(user);
//            Game.users.Reverse();

            await Login.SendCountsAndIsFirstToAll(this);

            await CloseSocket(socket);
        }

        public async Task SendMessageToAllAsync<T>(Message<T> message, List<User> list) where T : ISendableMessage
        {
            foreach (var socket in list)
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

        private async Task Run<T>(string message, WebSocket webSocket) where T : IRecievedMessage, new()
        {
            var obj = JsonConvert.DeserializeObject<Message<T>>(message);
            obj.Data = obj.Data == null ? new T() : obj.Data;
            await obj.Data.Run(this, webSocket);
        }

        public async Task RunCommandAsync(string message, WebSocket webSocket)
        {
            var decodedForName = JsonConvert.DeserializeObject<Message<object>>(message);
            switch (decodedForName.Name)
            {
                case "login":
                    await Run<Login>(message, webSocket);
                    break;
                case "click":
                    await Run<Click>(message, webSocket);
                    break;
                case "startGame":
                    await Run<StartGame>(message, webSocket);
                    break;
            }
        }
    }
}
