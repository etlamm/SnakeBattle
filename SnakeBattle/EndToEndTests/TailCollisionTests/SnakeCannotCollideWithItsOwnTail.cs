using System;
using System.Threading.Tasks;
using ConsoleApp;
using ConsoleApp.Views;
using Dependencies;
using Domain;
using Domain.Player;
using EndToEndTests.Player;

namespace EndToEndTests.TailCollisionTests
{
    class SnakeCannotCollideWithItsOwnTail : IEndToEndTest
    {
        private Game game;
        private MainView mainView;

        public string Category => Categories.TailCollision;
        public string Description => "Snake cannot collide with its own tail";

        public async Task Run()
        {
            var options = DependencyInjection.GetObject<GameOptions>();
            options.TestMode = true;
            options.EnableFood = false;
            options.EnableRounds = false;
            options.InitialSnakeLength = 40;        
            game = DependencyInjection.GetObject<Game>();

            mainView = new MainView(game);

            var center = Location.At(game.Map.Width / 2, game.Map.Height / 2);
            game.AddPlayers(new AddPlayerRequest(new TurningPlayer(Direction.East, 10), Location.At(center.X - 5, center.Y - 5), Direction.East));

            mainView.Render();

            mainView.ShowModalAndHideAfterKeyPress(new[] { "Press any key to start" });

            await new GameRunner(game, mainView).RunGame();
        }
    }
}
