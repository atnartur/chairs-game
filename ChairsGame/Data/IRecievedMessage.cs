using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChairsGame.Data
{
    public interface IRecievedMessage
    {
        Task Run(Global global, WebSocket webSocket);
    }
}