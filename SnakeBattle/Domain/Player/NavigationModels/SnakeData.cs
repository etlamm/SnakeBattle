using System.Collections.Generic;
using System.Linq;

namespace Domain.Player.NavigationModels
{
    public class SnakeData
    {
        private readonly Snake.Snake snake;

        public int PointsInCurrentRound => snake.PointsInCurrentRound;
        public Direction Direction => snake.Direction;
        /// <summary>
        /// Location of the snake's "head" (first part of the snake).
        /// </summary>
        public Location HeadLocation => snake.Head.Location;
        /// <summary>
        /// Location of the snake's "tail" (last part of the snake).
        /// </summary>
        public Location TailLocation => snake.Tail.Location;
        public List<Location> PartLocations => snake.Parts.Select(part => part.Location).ToList();
        public bool IsAlive => snake.IsAlive;

        public SnakeData(Snake.Snake snake)
        {
            this.snake = snake;
        }
    }
}
