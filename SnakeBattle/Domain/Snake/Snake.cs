using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Event;
using Domain.Player;
using Domain.Player.NavigationModels;

namespace Domain.Snake
{
    public class Snake : ISnake
    {
        public int Id { get; }
        public IPlayer Player { get; }
        public List<SnakePart> Parts { get; } = new();
        public SnakePart Head => Parts.First();
        public SnakePart Tail => Parts.Last();
        public Direction Direction { get; set; }
        public DirectionRequest DirectionRequest { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public bool IsDead => !IsAlive;
        public int PointsInCurrentRound { get; private set; }
        public int TotalPoints { get; private set; }
        public int DistanceTraveled { get; private set; }
        public int SecondsLived { get; private set; }
        private Location initialLocation;
        private readonly int initialSnakeLength;
        private readonly Direction initialDirection;
        private readonly IEventBus eventBus;
        public SnakeData SnakeDataForPlayer { get; }

        public Snake(int id, IPlayer player, Location location, int initialSnakeLength, Direction direction,
            IEventBus eventBus)
        {
            Id = id;
            Player = player;
            initialLocation = location;
            this.initialSnakeLength = initialSnakeLength;
            initialDirection = direction;
            Direction = direction;
            this.eventBus = eventBus;
            SnakeDataForPlayer = new SnakeData(this);
            CreateInitialParts();
        }

        private void CreateInitialParts()
        {
            for (int i = 0; i < initialSnakeLength; i++)
            {
                Parts.Add(new SnakePart(initialLocation));
            }

            eventBus.Send(new CellContentChanged(Head.Location));
        }

        public void Reset(Location initiaLocation)
        {
            Direction = initialDirection;
            this.initialLocation = initiaLocation;

            foreach (var part in Parts)
            {
                eventBus.Send(new CellContentChanged(part.Location));
            }

            Parts.Clear();
            CreateInitialParts();
            IsAlive = true;
            PointsInCurrentRound = 0;
            DistanceTraveled = 0;
            SecondsLived = 0;
            eventBus.Send(new AmountOfFoodEatenChanged(this));
        }

        public void Move()
        {
            if (IsDead)
            {
                return;
            }

            eventBus.Send(new CellContentChanged(Location.At(Tail.Location.X, Tail.Location.Y)));

            for (int i = Parts.Count - 1; i > 0; i--)
            {
                Parts[i].Location = Parts[i - 1].Location;
            }

            switch (Direction)
            {
                case Direction.East:
                    Head.Location = Head.Location.OneStepEastwards();
                    break;
                case Direction.South:
                    Head.Location = Head.Location.OneStepSouthwards();
                    break;
                case Direction.West:
                    Head.Location = Head.Location.OneStepWestwards();
                    break;
                case Direction.North:
                    Head.Location = Head.Location.OneStepNorthwards();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DistanceTraveled++;
            eventBus.Send(new CellContentChanged(Head.Location));
        }

        public void Die(int totalSecondsLived)
        {
            IsAlive = false;
            SecondsLived = totalSecondsLived;
        }

        public void EatFood()
        {
            PointsInCurrentRound++;
            TotalPoints++;
            for (int i = 0; i < 10; i++)
            {
                Parts.Add(new SnakePart(Tail.Location));
            }

            eventBus.Send(new AmountOfFoodEatenChanged(this));
        }

        public void SetNewDirection()
        {
            if (DirectionRequest.Direction != Direction.GetOppositeDirection())
            {
                Direction = DirectionRequest.Direction;
            }

            DirectionRequest.DirectionIsUsed = true;
        }

        public Location GetCellAhead()
        {
            return Head.Location.OneStepTo(Direction);
        }

        public bool IsReadyToAskForNewDirection() =>
            IsAlive && (
                DirectionRequest is null
                || DirectionRequest.IsTaskCompleted
                && DirectionRequest.DirectionIsUsed);

        public void AskForNewDirection(MapData mapData)
        {
            DirectionRequest = new DirectionRequest(Task.Run(() => Player.GetNextDirection(SnakeDataForPlayer, mapData)));
        }

        public bool HasReceivedNewDirection() => DirectionRequest.IsTaskCompletedSuccessfully && !DirectionRequest.DirectionIsUsed;
    }
}