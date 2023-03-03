using System.Collections.Generic;
using System.Linq;

namespace Domain.Player.NavigationModels
{
    public class MapData
    {
        private readonly Map.Map map;

        public Location FoodLocation => map.Food.Location;
        public bool LocationIsEmptyOrContainsFood(Location location) => map.LocationIsEmptyOrContainsFood(location);
        public bool LocationContainsFood(Location location) => map.LocationContainsFood(location);
        public bool LocationContainsObstacle(Location location) => map.LocationContainsObstacle(location);
        public List<SnakeData> OtherPlayersSnakes(SnakeData mySnakeData) => map.Snakes.Where(snake => snake.SnakeDataForPlayer != mySnakeData).Select(snake => snake.SnakeDataForPlayer).ToList();
        public List<SnakeData> AllSnakes => map.Snakes.Select(snake => snake.SnakeDataForPlayer).ToList();

        public MapData(Map.Map map)
        {
            this.map = map;
        }
    }
}
