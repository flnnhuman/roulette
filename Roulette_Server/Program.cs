using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roulette.Context;
using Roulette.Models;

namespace Roulette_Server
{
    public class Program
    {
        public static Queue<int> RollsHistory = new Queue<int>();
        public static List<BetModel> Bets = new List<BetModel>();

        public static IHubContext<NotificationHub> hubContext;
        public static AppDbContext AppDbContext;
        private static Timer timer;
        private static IHost host;


        public static void Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();
            hubContext = (IHubContext<NotificationHub>) host.Services.GetService(typeof(IHubContext<NotificationHub>));
            CreateTimer();


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(opt =>
                    {
                        AppDbContext = opt.ApplicationServices.CreateScope().ServiceProvider
                            .GetService<AppDbContext>();
                        RollsHistory = new Queue<int>(AppDbContext.GamesHistory
                            .Skip(Math.Max(0, AppDbContext.GamesHistory.Count() - 10))
                            .Select(model => model.WonNumber));
                    });
                });
        }


        public static void CreateTimer()
        {
            timer = new Timer(
                async e => { await Play(); },
                null,
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(27)
            );
        }

        private static async Task Play()
        {
            await NotificationHub.SendTimer(hubContext);
            await Task.Delay(TimeSpan.FromSeconds(20));

            var random = new Random();
            var rollValue = random.Next() % 15;
            Console.WriteLine(DateTime.Now + " Rolled " + rollValue);

            await NotificationHub.SendRollAsync(hubContext, rollValue.ToString());
            await Task.Delay(TimeSpan.FromSeconds(4));

            Color WonColor;
            List<BetModel> wonBets;
            if (rollValue == 0)
            {
                wonBets = Bets.Where(bet => bet.Color == Color.Green).ToList();
                WonColor = Color.Green;
            }

            else if (BetColors.Black.Any(x => x == rollValue))
            {
                wonBets = Bets.Where(bet => bet.Color == Color.Black).ToList();
                WonColor = Color.Black;
            }
            else
            {
                wonBets = Bets.Where(bet => bet.Color == Color.Red).ToList();
                WonColor = Color.Red;
            }


            var uniquePlayers = wonBets.Select(o => o.SteamID).Distinct();

            AppDbContext = Roulette.Program.GetNewContext();
            foreach (var player in uniquePlayers)
            {
                var betsOfOnePlayer = wonBets.Where(bet => bet.SteamID == player);
                var userModel = await AppDbContext.SteamUsers.Where(model => model.SteamID == player)
                    .FirstOrDefaultAsync();
                foreach (var bet in betsOfOnePlayer)
                {
                    var won = rollValue == 0 ? bet.Amount * 14 : bet.Amount * 2;
                    userModel.Balance += won;
                    userModel.TotalWon += won;
                }

                AppDbContext.SteamUsers.Update(userModel);
            }

            GameModel currentGame = new GameModel()
                {Timestamp = DateTime.UtcNow, WonNumber = rollValue, WonColor = WonColor, AllBets = Bets};
            await AppDbContext.GamesHistory.AddAsync(currentGame);
            await AppDbContext.SaveChangesAsync();
            Bets.Clear();
            if (RollsHistory.Count >= 10) RollsHistory.Dequeue();
            RollsHistory.Enqueue(rollValue);

            await NotificationHub.SendAllBetsAsync(hubContext);
            await NotificationHub.SendRollHistoryAsync(hubContext, string.Join(',', RollsHistory));
        }
    }
}