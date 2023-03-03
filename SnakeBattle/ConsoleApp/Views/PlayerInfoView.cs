using System;
using Domain.Snake;

namespace ConsoleApp.Views
{
    class PlayerInfoView : View
    {
        private readonly Position position;
        private readonly Size size;
        public ISnake Snake { get; }
        private int MaxVisibleTeamMembers => size.Height - 1;

        public PlayerInfoView(Position position, Size size, ISnake snake) : base(position, size)
        {
            this.position = position;
            this.size = size;
            Snake = snake;
        }

        public override void Render()
        {
            Console.ForegroundColor = Snake.GetColor();
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < Math.Min(Snake.Player.TeamMembers.Length, MaxVisibleTeamMembers); i++)
            {
                Console.SetCursorPosition(position.X, position.Y + i);
                var teamMember = Snake.Player.TeamMembers[i];
                Console.Write(teamMember.Substring(0, Math.Min(teamMember.Length, size.Width)));
            }

            RenderPoints();
        }

        public void RenderPoints()
        {
            Console.ForegroundColor = Snake.GetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(position.X, position.Y + size.Height - 1);
            Console.Write(Snake.PointsInCurrentRound.ToString().PadRight(size.Width));
        }
    }
}
