using Domain.Player;

namespace Domain.Snake
{
    public interface ISnakeFactory
    {
        ISnake Create(IPlayer player, Location location, int initialSnakeLength, Direction direction);
        ISnake Create(IPlayer player, Location location, int initialSnakeLength);
    }
}