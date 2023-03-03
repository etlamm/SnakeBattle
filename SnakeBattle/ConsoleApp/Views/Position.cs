using System;

namespace ConsoleApp.Views
{
    /// <summary>
    /// Represents a position in a console window.
    /// </summary>
    class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new position. The same as using a constructor, but this looks nicer in code.
        /// </summary>
        public static Position At(int x, int y)
        {
            return new Position(x, y);
        }

        public static Position operator +(Position left, Position right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return At(left.X + right.X, left.Y + right.Y);
        }
    }
}
