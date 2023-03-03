using System;
using Domain;
using Domain.Map;
using Domain.Snake;

namespace ConsoleApp.Views
{
    internal class MapView : View
    {
        private readonly Game game;

        public MapView(Game game, Position position, Size size) : base(position, size)
        {
            this.game = game;
        }

        private const char UpperHalfBlock = (char)9600;
        private const char LowerHalfBlock = (char)9604;
        private const char FullBlock = (char)9608;

        public override void Render()
        {
            RenderWalls();
            RenderSnakes();
            RenderFood();
        }

        public override void Render(Area area)
        {
            if (AreaIsOutsideView(area))
            {
                return;
            }

            for (int y = 0; y < area.Size.Height; y++)
            {
                for (int x = 0; x < area.Size.Width; x++)
                {
                    RenderConsolePosition(Position.At(area.Position.X + x, area.Position.Y + y));
                }
            }
        }

        private void RenderFood()
        {
            if (game.Map.HasFood())
            {
                RenderMapLocation(game.Map.Food.Location); 
            }
        }

        private void RenderSnakes()
        {
            foreach (var snake in game.Map.Snakes)
            {
                var location = snake.Head.Location;
                RenderMapLocation(location);
            }
        }

        private void RenderWalls()
        {
            for (int y = 0; y < Size.Width; y++)
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    if (PositionContainsWall(Position.At(x, y)))
                    {
                        var absolutePosition = Position.At(Position.X + x, Position.Y + y);
                        RenderConsolePosition(absolutePosition);
                    }
                }
            }
        }

        private bool PositionContainsWall(Position relativePosition)
        {
            return relativePosition.X == 0 || relativePosition.X == Size.Width - 1 || relativePosition.Y == 0 || relativePosition.Y == Size.Height - 1;
        }

        public void RenderConsolePosition(Position positionInConsole)
        {
            var mapLocationX = positionInConsole.X - Position.X;
            var mapLocationY = 2 * (positionInConsole.Y - Position.Y);
            RenderMapLocation(Location.At(mapLocationX, mapLocationY));
        }

        public Position GetPositionInView(Location location)
        {
            return Position.At(location.X, location.Y / 2);
        }

        public void RenderMapLocation(Location locationInMap)
        {
            var positionInView = GetPositionInView(locationInMap);
            var positionInConsole = positionInView + Position;
            if (PositionIsOutsideView(positionInConsole))
            {
                return;
            }

            var upperObjectLocation = Location.At(positionInView.X, positionInView.Y * 2);
            var upperObject = game.Map.GetVisibleObjectAt(upperObjectLocation);
            var lowerObject = game.Map.GetVisibleObjectAt(upperObjectLocation.OneStepSouthwards());
            Console.SetCursorPosition(positionInConsole.X, positionInConsole.Y);

            if (upperObject is Floor && lowerObject is not Floor)
            {
                Console.ForegroundColor = GetColor(lowerObject);
                Console.BackgroundColor = GetColor(upperObject);
                Console.Write(LowerHalfBlock);
            }
            else
            {
                Console.ForegroundColor = GetColor(upperObject);
                Console.BackgroundColor = GetColor(lowerObject);
                Console.Write(UpperHalfBlock);
            }
        }

        private ConsoleColor GetColor(IGameObject gameObject)
        {
            return gameObject switch
            {
                Wall => ConsoleColor.Gray,
                Floor => ConsoleColor.Black,
                Food when game.Map.Food.IsAboutToExpire && game.Map.Food.TimeLeft % 2 == 0 => ConsoleColor.Black,
                Food => ConsoleColor.White,
                Snake snake => snake.GetColor(),
                _ => ConsoleColor.DarkGray
            };
        }
    }
}