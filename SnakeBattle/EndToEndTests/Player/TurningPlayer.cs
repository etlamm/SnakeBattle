using System;
using Domain;
using Domain.Player;
using Domain.Player.NavigationModels;

namespace EndToEndTests.Player
{
    class TurningPlayer : IPlayer
    {
        private readonly Direction directionToTurn;
        private readonly int interval;
        private int stepCounter;

        public string[] TeamMembers => new[] { "Right turning player" };

        public TurningPlayer(Direction directionToTurn, int interval)
        {
            this.directionToTurn = directionToTurn;
            this.interval = interval;
        }

        public Direction GetNextDirection(SnakeData mySnake, MapData map)
        {
            if (stepCounter == interval)
            {
                stepCounter = 1;
                return directionToTurn switch
                {
                    Direction.West => mySnake.Direction.TurnLeft(),
                    Direction.East => mySnake.Direction.TurnRight(),
                    _ => throw new InvalidOperationException($"Invalid direction to turn: {directionToTurn}.")
                };
            }

            stepCounter++;
            return mySnake.Direction;
        }
    }
}
