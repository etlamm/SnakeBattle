using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Event;
using Domain.Player.NavigationModels;
using Domain.Snake;

namespace Domain.Map
{
    public class Map : IMap
    {
        public int Width { get; }
        public int Height { get; }
        public List<ISnake> Snakes { get; } = new();
        public Food Food { get; private set; }
        private readonly IEventBus eventBus;
        public MapData MapDataForPlayer { get; }
        private readonly Random random;

        public Map(GameOptions options, IEventBus eventBus)
        {
            Width = options.MapWidth;
            Height = options.MapHeight;
            random = new Random(options.RandomSeed);
            this.eventBus = eventBus;
            MapDataForPlayer = new MapData(this);
        }

        public IGameObject GetVisibleObjectAt(Location location) => GetObjectAt(location) ?? Floor.Instance;

        public IGameObject GetObjectAt(Location location)
        {
            if (location.X < 1 || location.X >= Width - 1 || location.Y < 1 || location.Y >= Height - 1)
            {
                return Wall.Instance;
            }
            if (Food?.Location == location)
            {
                return Food;
            }

            foreach (var snake in Snakes)
            {
                if (snake.Parts.Any(part => part.Location == location))
                {
                    return snake;
                }
            }

            return null;
        }


        public void RelocateFood()
        {
            eventBus.Send(new CellContentChanged(Food.Location));

            Food = new Food(FindRandomEmptyLocation(), eventBus);

            eventBus.Send(new CellContentChanged(Food.Location));
        }

        public void RelocateFood(Location newLocation)
        {
            eventBus.Send(new CellContentChanged(Food.Location));

            Food = new Food(newLocation, eventBus);

            eventBus.Send(new CellContentChanged(Food.Location));
        }

        private Location FindRandomEmptyLocation()
        {
            Location location;
            do
            {
                location = new Location(random.Next(Width - 2) + 1, random.Next(Height - 2) + 1);
            } while (!LocationIsEmpty(location) || GetDistanceToClosestSnake(location) < 10);
            return location;
        }

        private int GetDistanceToClosestSnake(Location location)
        {
            int closestDistance = Int32.MaxValue;
            foreach (var snake in Snakes.Where(snake => snake.IsAlive))
            {
                var distance = Math.Abs(snake.Head.Location.X - location.X) +
                               Math.Abs(snake.Head.Location.Y - location.Y);
                closestDistance = Math.Min(closestDistance, distance);
            }

            return closestDistance;
        }

        public bool LocationIsEmpty(Location location) => GetObjectAt(location) == null;

        public void AddFirstFood()
        {
            var randomLocation = FindRandomEmptyLocation();
            var y = Height * 2 / 3; // Set first food always on the lower part of the map
            Food = new Food(Location.At(randomLocation.X, y), eventBus);
        }

        public bool LocationContainsFood(Location location) => Food.Location == location;

        public bool LocationContainsObstacle(Location location)
        {
            var gameObject = GetObjectAt(location);
            return gameObject is Wall || gameObject is Snake.Snake;
        }

        public bool LocationIsEmptyOrContainsFood(Location location)
        {
            var gameObject = GetObjectAt(location);
            return gameObject is null or Domain.Map.Food;
        }

        public bool HasFood() => Food != null;

        public bool LocationContainsObstacleThatCausesCertainCollision(Location location)
        {
            var gameObject = GetObjectAt(location);
            return gameObject is Wall
                   || gameObject is Snake.Snake snake &&
                   (snake.IsDead || snake.Tail.Location != location);
        }

        public void AddSnake(ISnake snake)
        {
            Snakes.Add(snake);
            eventBus.Send(new CellContentChanged(snake.Head.Location));
        }
    }
}