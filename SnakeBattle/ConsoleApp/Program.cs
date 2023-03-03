using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConsoleApp.Views;
using Dependencies;
using Domain;

[assembly:InternalsVisibleTo("EndToEndTests")]

namespace ConsoleApp
{

    class Program
    {
        private static Game game;
        private static MainView mainView;
        private static Configuration configuration;
        private static GameOptions options;

        static async Task Main(string[] args)
        {
            ConfigureGame();
            game = DependencyInjection.GetObject<Game>();
            mainView = new MainView(game);
            new PlayerAdder(game, mainView, options).AddPlayers();
            mainView.Render();
            await new GameRunner(game, mainView).RunGame();
        }

        private static void ConfigureGame()
        {
            configuration = Configuration.ReadConfiguration();
            options = DependencyInjection.GetObject<GameOptions>();
            options.TestMode = configuration.TestMode ?? options.TestMode;
            options.MillisecondsBetweenMoves = configuration.MillisecondsBetweenMoves ?? options.MillisecondsBetweenMoves;
            options.Rounds = configuration.Rounds ?? options.Rounds;
            options.RandomSeed = configuration.RandomSeed ?? options.RandomSeed;
        }
    }
}
