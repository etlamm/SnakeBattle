using System.Collections.Generic;
using Domain.Player.NavigationModels;
using Domain.Snake;

namespace Domain.Map
{
    public interface IMap
    {
        void AddFirstFood();
        void AddSnake(ISnake snake);
        int Width { get; }
        int Height { get; }
        List<ISnake> Snakes { get; }
        Food Food { get; }
        MapData MapDataForPlayer { get; }
        bool LocationIsEmptyOrContainsFood(Location location);
        void RelocateFood();
        void RelocateFood(Location location);
        bool LocationContainsObstacleThatCausesCertainCollision(Location location);
        bool HasFood();
        IGameObject GetVisibleObjectAt(Location location);
    }
}
