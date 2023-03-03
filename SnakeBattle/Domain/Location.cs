using System;

namespace Domain
{
    /// <summary>
    /// Represents a location on the game map.
    /// </summary>
    public class Location
    {
        public int X { get; }
        public int Y { get; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new location. It's the same as using a constructor, but this looks nicer in code.
        /// </summary>
        public static Location At(int x, int y)
        {
            return new Location(x, y);
        }

        public Location OneStepNorthwards() => GoToDirection(Direction.North, 1);

        public Location OneStepEastwards() => GoToDirection(Direction.East, 1);

        public Location OneStepWestwards() => GoToDirection(Direction.West, 1);

        public Location OneStepSouthwards() => GoToDirection(Direction.South, 1);

        public Location OneStepTo(Direction direction) => GoToDirection(direction, 1);

        public Location GoToDirection(Direction direction, int distance)
        {
            return direction switch
            {
                Direction.North => new Location(X, Y - distance),
                Direction.East => new Location(X + distance, Y),
                Direction.South => new Location(X, Y + distance),
                Direction.West => new Location(X - distance, Y),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static bool operator ==(Location locationA, Location locationB)
        {
            if (locationA is null || locationB is null)
            {
                return false;
            }

            return locationA.X == locationB.X && locationA.Y == locationB.Y;
        }

        public static bool operator !=(Location locationA, Location locationB)
        {
            return !(locationA == locationB);
        }
    }
}