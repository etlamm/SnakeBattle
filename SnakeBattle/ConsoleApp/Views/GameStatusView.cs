using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Snake;

namespace ConsoleApp.Views
{
    internal class GameStatusView : View
    {
        private readonly Game game;
        private readonly List<PlayerInfoView> playerInfoViews = new();
        public RoundInfoView RoundInfoView { get; }

        public GameStatusView(Game game, Position position, Size size) : base(position, size)
        {
            this.game = game;
            RoundInfoView = new RoundInfoView(game, Position.At(Position.X + Size.Width - RoundInfoView.GetViewWidth(), Position.Y), Size.Height);
        }

        private void CreatePlayerInfoViews()
        {
            var distanceBetweenPlayerInfoViews = ((float)Size.Width - RoundInfoView.GetViewWidth()) / game.Players.Count;

            for (var i = 0; i < game.Players.Count; i++)
            {
                var snake = game.Map.Snakes[i];
                var currentPlayerInfoLocationX = Position.X + (int)(i * distanceBetweenPlayerInfoViews);
                var currentPlayerInfoAreaWidth = (int)((i + 1) * distanceBetweenPlayerInfoViews) - (int)(i * distanceBetweenPlayerInfoViews) - 1;
                playerInfoViews.Add(new PlayerInfoView(Position.At(currentPlayerInfoLocationX, Position.Y), Size.Of(currentPlayerInfoAreaWidth, Size.Height), snake));
            }
        }
        
        public override void Render()
        {
            CreatePlayerInfoViews();

            foreach (var playerInfoView in playerInfoViews)
            {
                playerInfoView.Render();
            }

            RoundInfoView.Render();
        }

        public void RenderPoints(Snake snake)
        {
            playerInfoViews.Single(view => view.Snake == snake).RenderPoints();
        }
    }
}