using System;
using Domain;

namespace ConsoleApp.Views
{
    class RoundInfoView : View
    {
        private readonly Game game;

        public RoundInfoView(Game game, Position position, int height)
            : base(position, Size.Of(GetViewWidth(), height))
        {
            this.game = game;
        }

        private static string GetRoundText(int round)
        {
            return $"Round {round}";
        }

        public static int GetViewWidth() => GetRoundText(999).Length;

        public override void Render()
        {
            RenderRound();
            RenderStopwatch();
        }

        public void RenderStopwatch()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            var stopwatchText = GetStopwatchText();
            Console.SetCursorPosition(Position.X + Size.Width - stopwatchText.Length, Position.Y + 1);
            Console.Write(stopwatchText);
        }

        private string GetStopwatchText()
        {
            var totalSecondsElapsed = game.GetSecondsElapsedInRound();
            return $"{(totalSecondsElapsed / 60).ToString().PadLeft(2, '0')}:{(totalSecondsElapsed % 60).ToString().PadLeft(2, '0')}";
        }

        public void RenderRound()
        {
            var roundText = GetRoundText(game.Round);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(Position.X + Size.Width - roundText.Length, Position.Y);
            Console.Write(roundText);
        }
    }
}
