using System;
using Domain.Player.NavigationModels;

namespace Domain.Player
{
    /// <summary>
    /// A keyboard-controller player. This class is provided so that you can test
    /// how well your custom player implementation performs against a human player.
    /// </summary>
    public class KeyboardPlayer : IPlayer
    {
        public string[] TeamMembers => new[] { "Keyboard player" };

        public Direction GetNextDirection(SnakeData mySnake, MapData map)
        {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    return Direction.East;
                case ConsoleKey.DownArrow:
                    return Direction.South;
                case ConsoleKey.LeftArrow:
                    return Direction.West;
                case ConsoleKey.UpArrow:
                    return Direction.North;
                case ConsoleKey.Escape:
                    Console.ResetColor();
                    Console.SetCursorPosition(0, 0);
                    Environment.Exit(0);
                    return mySnake.Direction; // Can't go here but need to have it.
                default:
                    return mySnake.Direction;
            }
        }
    }
}
