using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChairsGame
{
    public class User
    {
        public WebSocket Socket { get; set; }
        public string Username { get; set; }
        public bool First { get; set; }
    }
}
