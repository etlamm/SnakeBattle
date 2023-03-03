using Domain.Player.NavigationModels;

namespace Domain.Player
{
    public interface IPlayer
    {
        /// <summary>
        /// Names of the team members implementing the player.
        /// </summary>
        public string[] TeamMembers { get; }

        /// <summary>
        /// Gets the next direction for your snake based on the current game status.
        /// </summary>
        /// <param name="mySnake">Your team's snake</param>
        /// <param name="map">The map containing all the snakes, food and walls</param>
        /// <returns></returns>
        public Direction GetNextDirection(SnakeData mySnake, MapData map);
    }
}
