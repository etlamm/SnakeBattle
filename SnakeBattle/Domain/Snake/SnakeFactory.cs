using Domain.Player;

namespace Domain.Snake
{
    public class SnakeFactory : ISnakeFactory
    {
        private int nextId = 1;
        private readonly IEventBus eventBus;

        public SnakeFactory(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public ISnake Create(IPlayer player, Location location, int initialSnakeLength, Direction direction)
        {
            return new Snake(nextId++, player, location, initialSnakeLength, direction, eventBus);
        }

        public ISnake Create(IPlayer player, Location location, int initialSnakeLength)
            => Create(player, location, initialSnakeLength, Direction.South);
    }
}