using System;
using System.Threading.Tasks;
using ConsoleApp;
using ConsoleApp.Views;
using Dependencies;
using Domain;
using Domain.Player;
using EndToEndTests.Player;

namespace EndToEndTests.HeadCollisionTests
{
    class FourSnakesCollide : IEndToEndTest
    {
        private Game game;
        private MainView mainView;

        public string Category => Categories.HeadCollision;
        public string Description => "Four snakes";

        public async Task Run()
        {
            var options = DependencyInjection.GetObject<GameOptions>();
            options.TestMode = true;
            options.EnableFood = false;
            options.EnableRounds = false;
            options.InitialSnakeLength = 2;
            game = DependencyInjection.GetObject<Game>();

            mainView = new MainView(game);

            var center = Location.At(game.Map.Width / 2, game.Map.Height / 2);
            game.AddPlayers(
                new AddPlayerRequest(new ForwardGoingPlayer(), Location.At(center.X, center.Y - 10), Direction.South),
                new AddPlayerRequest(new ForwardGoingPlayer(), Location.At(center.X, center.Y + 10), Direction.North),
                new AddPlayerRequest(new ForwardGoingPlayer(), Location.At(center.X + 10, center.Y), Direction.West),
                new AddPlayerRequest(new ForwardGoingPlayer(), Location.At(center.X - 10, center.Y), Direction.East)
                );

            mainView.Render();

            mainView.ShowModalAndHideAfterKeyPress(new[] { "Press any key to start" });

            await new GameRunner(game, mainView).RunGame();
        }
    }
}
