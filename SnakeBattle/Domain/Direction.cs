using System;

namespace Domain
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionExtensions
    {
        /// <summary>
        /// The method takes in a direction, turns left 90 degrees (rotates counterclockwise) and returns the resulting direction.
        /// </summary>
        /// <param name="currentDirection">Current direction</param>
        /// <returns>Direction after turning left</returns>
        public static Direction TurnLeft(this Direction currentDirection)
        {
            return currentDirection switch
            {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
            };
        }

        /// <summary>
        /// The method takes in a direction, turns right 90 degrees (rotates clockwise) and returns the resulting direction.
        /// </summary>
        /// <param name="currentDirection">Current direction</param>
        /// <returns>Direction after turning right</returns>
        public static Direction TurnRight(this Direction currentDirection)
        {
            return currentDirection switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
            };
        }

        public static Direction GetOppositeDirection(this Direction currentDirection)
        {
            return currentDirection switch
            {
                Direction.East => Direction.West,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                Direction.North => Direction.South,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
            };
        }
    }
}