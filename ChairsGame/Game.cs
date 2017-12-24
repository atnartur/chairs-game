using System.Collections.Generic;

namespace ChairsGame
{
    public class Game
    {
        public List<User> users = new List<User>();
        public List<User> queue = new List<User>();
        public bool IsStart { get; set; }
    }
}
