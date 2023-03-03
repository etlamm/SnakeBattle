using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain;
using Domain.Event;

namespace ConsoleApp.Views
{
    internal class MainView : View
    {
        public MapView MapView { get; }
        public GameStatusView GameStatusView { get; }
        private ModalView modalView;
        public Game Game { get; }
        private const int GameStatusViewHeight = 4;
        private bool wholeMainViewHasBeenRendered;

        public MainView(Game game) : base(Position.At(0, 0), GetViewSize(game))
        {
            Game = game;
            ConfigureWindowAndConsole();
            var mapViewHeight = game.Map.Height / 2;
            GameStatusView = new GameStatusView(game, Position.At(Position.X, Position.Y), Size.Of(Size.Width, GameStatusViewHeight));
            MapView = new MapView(game, Position.At(Position.X, GameStatusView.Size.Height), Size.Of(Size.Width, mapViewHeight));
        }

        private static Size GetViewSize(Game game)
        {
            return Size.Of(game.Map.Width, game.Map.Height / 2 + GameStatusViewHeight * 2);
        }

        public override void Render()
        {
            GameStatusView.Render();
            MapView.Render();
            modalView?.Render();
            wholeMainViewHasBeenRendered = true;
        }

        public override void Render(Area area)
        {
            GameStatusView.Render(area);
            MapView.Render(area);
            modalView?.Render(area);
        }

        private void ConfigureWindowAndConsole()
        {
            Console.WindowWidth = Size.Width;
            Console.WindowHeight = Size.Height;
            Window.DisableResizing();
            Console.OutputEncoding = Encoding.UTF8;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            Console.CursorVisible = false;
        }

        public void ShowModal(IEnumerable<string> message)
        {
            modalView = new ModalView(message.ToArray());
            modalView.Render();
        }

        public void ShowModalAndHideAfterKeyPress(IEnumerable<string> message)
        {
            ShowModal(message);
            Console.ReadKey(true);
            HideModal();
        }

        public void ShowModalAndHideAfterSecond(IEnumerable<string> message)
        {
            ShowModal(message);
            Thread.Sleep(1000);
            HideModal();
        }

        public void HideModal()
        {
            if (modalView is null)
            {
                return;
            }

            if (wholeMainViewHasBeenRendered)
            {
                var modalViewArea = modalView.Area;
                modalView = null;
                Render(modalViewArea);
            }
            else
            {
                modalView = null;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            }
        }

        public void Render(List<IEvent> gameEvents)
        {
            foreach (var gameEvent in gameEvents)
            {
                if (gameEvent is CellContentChanged cellContentChangedGameEvent)
                {
                    MapView.RenderMapLocation(cellContentChangedGameEvent.Location);
                }
                else if (gameEvent is FoodIsAboutToExpire)
                {
                    MapView.RenderMapLocation(Game.Map.Food.Location);
                }
                else if (gameEvent is AmountOfFoodEatenChanged snakeAteFood)
                {
                    GameStatusView.RenderPoints(snakeAteFood.Snake);
                }
                else if (gameEvent is StopwatchUpdated)
                {
                    GameStatusView.RoundInfoView.RenderStopwatch();
                }
                else if (gameEvent is NewRoundStarted)
                {
                    GameStatusView.RoundInfoView.RenderRound();
                }
            }
        }
    }
}