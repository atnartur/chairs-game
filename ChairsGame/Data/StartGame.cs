using System;
using System.Net.WebSockets;
using System.Timers;
using System.Threading.Tasks;
using System.Linq;

namespace ChairsGame.Data
{
    public class StartGame : IRecievedMessage
    {
        int k;
        public async Task Run(Global global, WebSocket webSocket)
        {
            global.Game.IsStart = true;
            var rnd = new Random();
            var timer = new Timer(1000);
            int timeLeft = rnd.Next(5, 15);

            await Send(global);
            
            timer.Start();
            timer.Elapsed += Timer_Elapsed;

            while (true)
                if (k > timeLeft)
                {
                    k = 0;
                    timer.Stop();
                    break;
                }

            await global.SendMessageToAllAsync(new Message<Nothing>()
            {
                Name = "musicStop",
                Data = null
            }, global.Game.users);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => k++;

        public static async Task Send(Global global)
        {
            await global.SendMessageToAllAsync(new Message<StartGameEntity>()
            {
                Name = "startGame",
                Data = new StartGameEntity(global.Game.users.Count(x => !x.IsKicked) - 1)
            }, global.Game.users);
        }
    }
}