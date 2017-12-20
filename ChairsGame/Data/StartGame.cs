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
            var rnd = new Random();
            var timer = new Timer(1000);
            int timeLeft = rnd.Next(5, 15);

            await global.SendMessageToAllAsync(new Message<StartGameEntity>()
            {
                Name = "startGame",
                Data = new StartGameEntity()
                {
                    CountOfChairs = global.Game.users.Where(x => x.IsKicked == false).Count()
                }
            });

            timer.Start();
            timer.Elapsed += Timer_Elapsed;

            while (true)
                if (k > timeLeft)
                {
                    k = 0;
                    timer.Stop();
                    break;
                }

            await global.SendMessageToAllAsync(new Message<StartGameEntity>()
            {
                Name = "musicStop",
                Data = null
            });

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => k++;
    }
}