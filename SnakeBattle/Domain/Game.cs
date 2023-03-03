using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Event;
using Domain.Map;
using Domain.Player;
using Domain.Snake;

namespace Domain
{
    public class Game
    {
        public IMap Map { get; }
        public List<IPlayer> Players { get; } = new();
        public int Round { get; private set; } = 1;
        private readonly GameOptions options;
        public bool TestMode => options.TestMode;
        public bool EnableRounds => options.EnableRounds;
        private int movesSinceLastFoodEaten;
        private ISnakeFactory snakeFactory;
        private List<Location> initialSnakeLocations = new();
        public IEventBus EventBus { get; }
        private readonly Random random = new(DateTime.Now.Millisecond);
        private readonly Stopwatch stopwatch;
        public int TotalSecondsPlayed { get; private set; }
        public int NumberOfFullRoundsPlayed => AnySnakeIsAlive() ? Round - 1 : Round;

        public bool RoundEndedBecauseNoOneAteFoodInTooLongTime() =>
            movesSinceLastFoodEaten == options.MaxMovesToWaitAfterFoodEatenBeforeEndingRound;

        public Game(GameOptions options, IMap map, IEventBus eventBus, ISnakeFactory snakeFactory)
        {
            this.options = options;
            Map = map;
            EventBus = eventBus;
            this.snakeFactory = snakeFactory;
            stopwatch = new Stopwatch(EventBus);
        }

        /// <summary>
        /// Adds players and initializes the game.
        /// </summary>
        /// <param name="players">Players to add</param>
        public void AddPlayers(params IPlayer[] players)
        {
            foreach (var player in players)
            {
                Players.Add(player);
            }

            CreateSnakesOnMap();
            if (options.EnableFood)
            {
                Map.AddFirstFood();
            }
        }

        /// <summary>
        /// Adds players and initializes the game.
        /// </summary>
        /// <param name="requests">Data for initializing players</param>
        public void AddPlayers(params AddPlayerRequest[] requests)
        {
            foreach (var request in requests)
            {
                Players.Add(request.Player);
                var snake = snakeFactory.Create(request.Player, request.Location, options.InitialSnakeLength, request.Direction);
                Map.AddSnake(snake);
            }

            if (options.EnableFood)
            {
                Map.AddFirstFood();
            }
        }

        private void CreateSnakesOnMap()
        {
            DefineInitialSnakeLocations();
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                var snake = snakeFactory.Create(player,
                    initialSnakeLocations[i],
                    options.InitialSnakeLength);
                Map.AddSnake(snake);
            }
        }

        private void DefineInitialSnakeLocations()
        {
            var distanceBetweenSnakes = (float)Map.Width / (Players.Count + 1);
            for (int i = 0; i < Players.Count; i++)
            {
                initialSnakeLocations.Add(Location.At((int)((i + 1) * distanceBetweenSnakes), Map.Height / 4));
            }
            RandomizeInitialSnakeLocationsIfNotInTestMode();
        }

        private void RandomizeInitialSnakeLocationsIfNotInTestMode()
        {
            if (!options.TestMode)
            {
                initialSnakeLocations = initialSnakeLocations.Shuffle(random);
            }
        }

        public void Progress()
        {
            if (!AnySnakeIsAlive())
            {
                return;
            }

            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            AskForNewDirections();
            System.Threading.Thread.Sleep(options.MillisecondsBetweenMoves);
            WaitForDirectionResponsesIfInTestMode();
            SetSnakeDirections();
            MoveSnakes();
            KillAllSnakesIfFoodHasNotBeenEatenInLongTime();
            HandleAllSnakesHaveDied();
            HandleFoodExpiration();
        }

        private void HandleAllSnakesHaveDied()
        {
            if (Map.Snakes.All(snake => snake.IsDead))
            {
                stopwatch.Stop();
                TotalSecondsPlayed += stopwatch.TotalSecondsElapsed;
            }
        }

        private void KillAllSnakesIfFoodHasNotBeenEatenInLongTime()
        {
            if (movesSinceLastFoodEaten >= options.MaxMovesToWaitAfterFoodEatenBeforeEndingRound)
            {
                var totalSecondsElapsed = stopwatch.TotalSecondsElapsed;
                foreach (var snake in Map.Snakes.Where(snake => snake.IsAlive))
                {
                    snake.Die(totalSecondsElapsed);
                }
            }
        }

        private void MoveSnakes()
        {
            List<ISnake> allSnakesThatHaveMoved = new();
            List<ISnake> snakesThatMovedInThisIteration;
            List<ISnake> snakesThatMightStillMove;
            List<ISnake> killedSnakes;
            var snakesInRandomOrder = Map.Snakes.Shuffle(random);
            do
            {
                snakesThatMightStillMove =
                    snakesInRandomOrder.Where(snake => snake.IsAlive && !allSnakesThatHaveMoved.Contains(snake)).ToList();
                killedSnakes = KillSnakesThatCannotAvoidCollision(snakesThatMightStillMove, allSnakesThatHaveMoved);
                snakesThatMovedInThisIteration = MoveSnakesThatHaveEmptyCellOrFoodAhead(snakesThatMightStillMove);
                foreach (var snake in snakesThatMovedInThisIteration)
                {
                    snakesThatMightStillMove.Remove(snake);
                }

                allSnakesThatHaveMoved.AddRange(snakesThatMovedInThisIteration);
            } while (killedSnakes.Count > 0 || snakesThatMovedInThisIteration.Count > 0);

            foreach (var snake in snakesThatMightStillMove)
            {
                MoveSnake(snake);
            }

            movesSinceLastFoodEaten++;
        }

        /// <summary>
        /// If in test mode, wait for all direction requests to finish to help debugging. Otherwise stopping
        /// at break point will cause the direction response to be lost. 
        /// </summary>
        private void WaitForDirectionResponsesIfInTestMode()
        {
            if (TestMode)
            {
                // TODO: Use Task.WaitAll()?
                while (Map.Snakes.Any(snake => snake.Player is not KeyboardPlayer && !snake.DirectionRequest.IsTaskCompleted)) ;
            }
        }

        private void MoveSnake(ISnake snake)
        {
            snake.Move();
            if (SnakeReachedFood(snake))
            {
                SnakeEatsFood(snake);
            }
        }

        private void SnakeEatsFood(ISnake snake)
        {
            snake.EatFood();
            Map.RelocateFood();
            movesSinceLastFoodEaten = 0;
        }

        private List<ISnake> MoveSnakesThatHaveEmptyCellOrFoodAhead(List<ISnake> snakes)
        {
            var movedSnakes = new List<ISnake>();

            foreach (var snake in snakes)
            {
                var cellAhead = snake.GetCellAhead();
                if (Map.LocationIsEmptyOrContainsFood(cellAhead))
                {
                    MoveSnake(snake);
                    movedSnakes.Add(snake);
                }
            }

            return movedSnakes;
        }

        private List<ISnake> KillSnakesThatCannotAvoidCollision(List<ISnake> snakesThatMightStillMove, List<ISnake> allSnakesThatHaveMoved)
        {
            var killedSnakes = new List<ISnake>();

            foreach (var snake in snakesThatMightStillMove)
            {
                var cellAhead = snake.GetCellAhead();
                if (Map.LocationContainsObstacleThatCausesCertainCollision(cellAhead))
                {
                    var totalSecondsElapsed = stopwatch.TotalSecondsElapsed;
                    snake.Die(totalSecondsElapsed);
                    killedSnakes.Add(snake);

                    var otherSnake = allSnakesThatHaveMoved.SingleOrDefault(otherSnake => otherSnake.IsAlive && otherSnake.Head.Location == cellAhead);
                    if (otherSnake != null)
                    {
                        otherSnake.Die(totalSecondsElapsed);
                        killedSnakes.Add(otherSnake);
                    }
                }
            }

            return killedSnakes;
        }

        private void HandleFoodExpiration()
        {
            if (Map.HasFood())
            {
                Map.Food.ReduceTimeToExpiration();
                RelocateFoodIfTimerExpired(); 
            }
        }

        private void RelocateFoodIfTimerExpired()
        {
            if (Map.Food.IsExpired)
            {
                Map.RelocateFood();
            }
        }

        private bool SnakeReachedFood(ISnake snake) => snake.Head.Location == Map.Food?.Location;

        private void AskForNewDirections()
        {
            foreach (var snake in Map.Snakes.Where(snake => snake.IsAlive))
            {
                if (snake.IsReadyToAskForNewDirection())
                {
                    snake.AskForNewDirection(Map.MapDataForPlayer);
                }
            }
        }

        private void SetSnakeDirections()
        {
            foreach (var snake in Map.Snakes.Where(snake => snake.IsAlive))
            {
                if (snake.HasReceivedNewDirection())
                {
                    snake.SetNewDirection();
                }
                else
                {
                    Debug.WriteLine($"{DateTime.Now} {snake.Player.GetType()} failed to give a direction.");
                }
            }
        }

        public bool AnySnakeIsAlive()
        {
            return Map.Snakes.Any(snake => snake.IsAlive);
        }

        public bool IsTie()
        {
            if (Map.Snakes.Count < 2)
            {
                return false;
            }

            var orderedSnakes = Map.Snakes.OrderByDescending(snake => snake.PointsInCurrentRound).ToList();

            return orderedSnakes[0].PointsInCurrentRound == orderedSnakes[1].PointsInCurrentRound;
        }

        private List<ISnake> GetSnakesOrderedByScore()
        {
            var orderedSnakes = Map.Snakes
                .OrderByDescending(snake => snake.PointsInCurrentRound)
                .ThenByDescending(snake => snake.DistanceTraveled).ToList();
            return orderedSnakes;
        }

        public ISnake GetWinner() => GetSnakesOrderedByScore().First();

        public List<ISnake> GetWinnersInATie()
        {
            var orderedSnakes = GetSnakesOrderedByScore();
            var firstSnake = orderedSnakes.First();
            return orderedSnakes.Where(snake => snake.PointsInCurrentRound == firstSnake.PointsInCurrentRound).ToList();

        }

        public int GetSecondsElapsedInRound() => stopwatch.TotalSecondsElapsed;

        public void InitNewRound()
        {
            Round++;
            EventBus.Send(NewRoundStarted.Instance);
            ResetSnakes();
            stopwatch.Reset();
            movesSinceLastFoodEaten = 0;

            if (options.EnableFood)
            {
                Map.RelocateFood();
            }
        }

        public void ResetSnakes()
        {
            RandomizeInitialSnakeLocationsIfNotInTestMode();
            for (var i = 0; i < Map.Snakes.Count; i++)
            {
                Map.Snakes[i].Reset(initialSnakeLocations[i]);
            }
        }

        public bool AllRoundsHaveBeenPlayed()
        {
            return NumberOfFullRoundsPlayed == options.Rounds;
        }

        public bool IsFinalRound() => Round == options.Rounds;

        public void StopStopwatch()
        {
            stopwatch.Stop();
        }

        public void StartStopwatch()
        {
            stopwatch.Start();
        }
    }
}
