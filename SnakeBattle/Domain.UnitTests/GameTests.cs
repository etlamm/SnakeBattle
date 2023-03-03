using System.Collections.Generic;
using Domain.Map;
using Domain.Snake;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UnitTests
{
    public class GameTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetsWinner()
        {
            var map = Substitute.For<IMap>();
            var snake1 = Substitute.For<ISnake>();
            var snake2 = Substitute.For<ISnake>();
            snake1.PointsInCurrentRound.Returns(1);
            snake2.PointsInCurrentRound.Returns(2);
            map.Snakes.Returns(new List<ISnake>
            {
                snake1,
                snake2,
            });
            var game = new Game(new GameOptions(), map, Substitute.For<IEventBus>(), Substitute.For<ISnakeFactory>());

            var winner = game.GetWinner();

            winner.Should().Be(snake2);
        }

        [TestCase(new[] { 0, 0 }, true)]
        [TestCase(new[] { 1, 1 }, true)]
        [TestCase(new[] { 1, 1, 0 }, true)]
        [TestCase(new[] { 1, 0 }, false)]
        [TestCase(new[] { 1, 0, 0 }, false)]
        public void DetectsTie(int[] pointsArray, bool isExpectedToBeTie)
        {
            var map = Substitute.For<IMap>();
            var snakes = new List<ISnake>();
            foreach (var points in pointsArray)
            {
                var snake = Substitute.For<ISnake>();
                snake.PointsInCurrentRound.Returns(points);
                snakes.Add(snake);
            }

            map.Snakes.Returns(snakes);
            var game = new Game(new GameOptions(), map, Substitute.For<IEventBus>(), Substitute.For<ISnakeFactory>());

            var isTie = game.IsTie();

            isTie.Should().Be(isExpectedToBeTie);
        }
    }
}