using System.Collections.Generic;
using Domain.Player;
using Domain.Player.NavigationModels;

namespace Domain.Snake
{
    public interface ISnake : IGameObject
    {
        List<SnakePart> Parts { get; }
        bool IsAlive { get; }
        SnakePart Head { get; }
        bool IsDead { get; }
        IPlayer Player { get; }
        DirectionRequest DirectionRequest { get; }
        int PointsInCurrentRound { get; }
        int DistanceTraveled { get; }
        int SecondsLived { get; }
        int TotalPoints { get; }
        SnakeData SnakeDataForPlayer { get; }
        Direction Direction { get; set; }
        SnakePart Tail { get; }
        int Id { get; }
        void Die(int totalSecondsElapsed);
        Location GetCellAhead();
        void Move();
        bool IsReadyToAskForNewDirection();
        void AskForNewDirection(MapData mapData);
        void EatFood();
        bool HasReceivedNewDirection();
        void SetNewDirection();
        void Reset(Location initialSnakeLocation);
    }
}
