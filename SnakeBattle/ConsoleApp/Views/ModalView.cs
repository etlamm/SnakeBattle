using System;
using System.Linq;

namespace ConsoleApp.Views
{
    class ModalView : View
    {
        private readonly string[] message;
        private const int HorizontalMargin = 2;
        private const int VerticalMargin = 1;

        public ModalView(string[] message) : base(GetPosition(message), GetSize(message))
        {
            this.message = message;
        }

        private static Position GetPosition(string[] message)
        {
            var size = GetSize(message);
            return Position.At(Console.WindowWidth / 2 - size.Width / 2, Console.WindowHeight / 2 - size.Height / 2);
        }

        private static Size GetSize(string[] message)
        {
            var maxRowLength = message.OrderByDescending(row => row.Length).First().Length;
            var rows = message.Length;
            return Size.Of(maxRowLength + HorizontalMargin * 2, rows + VerticalMargin * 2);
        }

        public override void Render()
        {
            RenderBackground();
            RenderMessage();
        }

        private void RenderBackground()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < Size.Height; i++)
            {
                Console.SetCursorPosition(Position.X, Position.Y + i);
                Console.Write("".PadRight(Size.Width));
            }
        }

        private void RenderMessage()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (var i = 0; i < message.Length; i++)
            {
                Console.ForegroundColor = i == 0
                    ? ConsoleColor.Yellow
                    : ConsoleColor.Cyan;
                var row = message[i];
                Console.SetCursorPosition(Position.X + Size.Width / 2 - row.Length / 2, Position.Y + VerticalMargin + i);
                Console.Write(row);
            }
        }
    }
}
