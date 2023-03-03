using Domain.Player;
using Domain.Player.NavigationModels;

namespace Domain.IntegrationTests
{
    class DummyPlayer : IPlayer
    {
        public string[] TeamMembers => new[] { "Dummy" };
        public Direction GetNextDirection(SnakeData mySnake, MapData map)
        {
            return mySnake.Direction;
        }
    }
}