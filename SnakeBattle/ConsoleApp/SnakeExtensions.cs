using System;
using Domain.Snake;

namespace ConsoleApp
{
    public static class SnakeExtensions
    {
        public static ConsoleColor GetColor(this ISnake snake)
        {
            const int numberOfColors = 13;
            return (snake.Id % numberOfColors) switch
            {
                1 => ConsoleColor.Red,
                2 => ConsoleColor.Magenta,
                3 => ConsoleColor.Blue,
                4 => ConsoleColor.Green,
                5 => ConsoleColor.Yellow,
                6 => ConsoleColor.Cyan,
                7 => ConsoleColor.DarkRed,
                8 => ConsoleColor.DarkMagenta,
                9 => ConsoleColor.DarkBlue,
                10 => ConsoleColor.DarkGreen,
                11 => ConsoleColor.DarkYellow,
                12 => ConsoleColor.DarkCyan,
                13 => ConsoleColor.DarkGray
            };
        }
    }
}