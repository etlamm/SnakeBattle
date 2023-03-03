using System.Linq;
using Dependencies;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.IntegrationTests
{
    public class PlayerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SnakeDataReturnsCorrectData()
        {
            var game = DependencyInjection.GetObject<Game>();

            var testPlayer = new DummyPlayer();
            game.AddPlayers(testPlayer, new DummyPlayer(), new DummyPlayer());

            var snake = game.Map.Snakes.Single(snake => snake.Player == testPlayer);
            var snakeData = game.Map.Snakes.Single(snake => snake.Player == testPlayer).SnakeDataForPlayer;

            snakeData.PointsInCurrentRound.Should().Be(0);
            snake.EatFood();
            snakeData.PointsInCurrentRound.Should().Be(1);

            snakeData.IsAlive.Should().BeTrue();
            snake.Die(0);
            snakeData.IsAlive.Should().BeFalse();

            snakeData.Direction.Should().Be(Direction.South);
            snake.Direction = Direction.East;
            snakeData.Direction.Should().Be(Direction.East);

            snakeData.HeadLocation.Should().Be(snake.Head.Location);
            snakeData.TailLocation.Should().Be(snake.Tail.Location);

            for (var i = 0; i < snakeData.PartLocations.Count; i++)
            {
                snakeData.PartLocations[i]
                    .Should().Be(snake.Parts[i].Location);
            }
        }

        [Test]
        public void MapDataReturnsCorrectData()
        {
            var game = DependencyInjection.GetObject<Game>();

            var testPlayer = new DummyPlayer();
            game.AddPlayers(testPlayer, new DummyPlayer(), new DummyPlayer());

            game.Map.RelocateFood(Location.At(60, 30));
            var mapData = game.Map.MapDataForPlayer;
            var snakeData = game.Map.Snakes.Single(snake => snake.Player == testPlayer).SnakeDataForPlayer;

            var otherPlayersSnakes = mapData.OtherPlayersSnakes(snakeData);
            otherPlayersSnakes.Count.Should().Be(2);
            otherPlayersSnakes.Any(snake => snake == snakeData).Should().BeFalse();

            mapData.AllSnakes.Count.Should().Be(3);
            mapData.AllSnakes.Single(snake => snake == snakeData);
            mapData.FoodLocation.Should().Be(game.Map.Food.Location);
            foreach (var part in mapData.AllSnakes.SelectMany(snake => snake.PartLocations))
            {
                mapData.LocationContainsObstacle(part).Should().Be(true);
                mapData.LocationIsEmptyOrContainsFood(part).Should().Be(false);
                mapData.LocationContainsFood(part).Should().Be(false);
            }
            mapData.LocationContainsFood(game.Map.Food.Location).Should().BeTrue();
            mapData.LocationContainsFood(game.Map.Food.Location.OneStepEastwards()).Should().BeFalse();
            mapData.LocationContainsObstacle(game.Map.Snakes.Last().Head.Location).Should().BeTrue();
            mapData.LocationContainsObstacle(Location.At(0, 0)).Should().BeTrue();
            mapData.LocationContainsObstacle(Location.At(1, 1)).Should().BeFalse();
            mapData.LocationIsEmptyOrContainsFood(game.Map.Food.Location).Should().BeTrue();
            mapData.LocationIsEmptyOrContainsFood(Location.At(1, 1)).Should().BeTrue();
            mapData.LocationIsEmptyOrContainsFood(Location.At(0, 0)).Should().BeFalse();
        }
    }
}