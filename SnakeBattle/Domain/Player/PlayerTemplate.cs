using Domain.Player.NavigationModels;

namespace Domain.Player
{
    // TODO: Duplicate this file and rename the copy as you like.
    // The game will automatically detect the new class and use it to control the snake.
    public class PlayerTemplate : IPlayer
    {
        // TODO: Enter team members names here.
        public string[] TeamMembers => new[] { "Name1", "Name2", "Name3" };

        public Direction GetNextDirection(SnakeData mySnake, MapData map)
        {
            // TODO: Implement logic for deciding the next direction for your snake.
            // The method should return the direction where you want your snake to go next.
            // The game runs a loop that calls this method repeatedly (every time your snake moves).
            // The parameters (mySnake and map) contain all the data you need for deciding the next direction.
            // Note that your snake can't turn to the opposite direction. E.g. if your snake is going south,
            // the next direction can't be north.

            // Your goals:
            // 1. Navigate to food
            // 2. Avoid obstacles

            // Example snippets for dealing with directions
            // The methods return a new direction, they do not modify the existing direction.
            var currentDirection = mySnake.Direction;
            var directionForTurningRight = mySnake.Direction.TurnRight(); // Turn right 90 degrees, i.e. rotate clockwise
            var oppositeDirection = mySnake.Direction.GetOppositeDirection();

            // Example snippets for dealing with locations
            // The methods return a new location, they do not modify the existing location.
            var nextCellInEast = mySnake.HeadLocation.OneStepEastwards();
            var cellAhead = mySnake.HeadLocation.OneStepTo(mySnake.Direction);
            var tenCellsAhead = mySnake.HeadLocation.GoToDirection(mySnake.Direction, 10);
            var xCoordinateOfHeadLocation = mySnake.HeadLocation.X;

            // Example snippets for inspecting the map
            var foodLocation = map.FoodLocation;
            var cellAheadIsEmptyOrContainsFood = map.LocationIsEmptyOrContainsFood(cellAhead);
            var locationContainsObstacle = map.LocationContainsObstacle(Location.At(12, 34));

            // See these classes for more details:
            // SnakeData
            // MapData
            // Location
            // Direction and DirectionExtensions

            var nextDirection = Direction.South;
            return nextDirection;
        }
    }
}
