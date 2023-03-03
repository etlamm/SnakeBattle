using Domain;
using Domain.Player;
using Domain.Player.NavigationModels;

namespace EndToEndTests.Player
{
    class ForwardGoingPlayer : IPlayer
    {
        public string[] TeamMembers => new[] { "Forward going player" };
        public Direction GetNextDirection(SnakeData mySnake, MapData map)
        {
            return mySnake.Direction;
        }
    }
}
