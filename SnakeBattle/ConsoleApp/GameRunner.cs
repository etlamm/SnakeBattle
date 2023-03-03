using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp.Views;
using Domain;
using Domain.Player;

namespace ConsoleApp
{
    class GameRunner
    {
        private readonly Game game;
        private readonly MainView mainView;
        private readonly MessageFactory messageFactory;
        private Task renderEventsTask;
        private List<string> latestTotalPointsSummary;

        public GameRunner(Game game, MainView mainView)
        {
            this.game = game;
            this.mainView = mainView;
            this.messageFactory = new MessageFactory(game);
        }

        public async Task RunGame()
        {
            ShowStartBattleModalForBattleMode();

            do
            {
                if (game.EnableRounds)
                {
                    ShowRoundStartModal();
                }

                while (game.AnySnakeIsAlive())
                {
                    game.Progress();
                    await RenderChangesAfterProgress();
                    QuitGameIfEscPressed();
                }

                if (game.EnableRounds)
                {
                    Task.WaitAll(renderEventsTask);
                    ShowTimeIsUpModal();
                    ShowRoundResultsModal();
                    ShowStandings();
                    if (!game.AllRoundsHaveBeenPlayed())
                    {
                        game.InitNewRound();
                        await RenderChanges();
                    }
                }
                else
                {
                    Console.ReadKey(true);
                    QuitGame();
                }
            } while (game.EnableRounds && !game.AllRoundsHaveBeenPlayed());
        }

        private void ShowTimeIsUpModal()
        {
            if (game.RoundEndedBecauseNoOneAteFoodInTooLongTime())
            {
                mainView.ShowModalAndHideAfterKeyPress(messageFactory.TimeIsUp());
            }
        }

        private void ShowStartBattleModalForBattleMode()
        {
            if (!game.TestMode)
            {
                mainView.ShowModalAndHideAfterKeyPress(messageFactory.StartingSnakeBattle());
            }
        }

        private async Task RenderChangesAfterProgress()
        {
            if (game.TestMode)
            {
                // Rendering must be done synchronously in test mode, because asynchronous rendering gets messed up when debugging.
                await RenderChanges();
            }
            else
            {
                RenderChanges();
            }
        }

        private void ShowRoundStartModal()
        {
            if (game.IsFinalRound())
            {
                mainView.ShowModalAndHideAfterSecond(messageFactory.FinalRound());
            }
            else
            {
                mainView.ShowModalAndHideAfterSecond(messageFactory.RoundStart());
            }
        }

        private void ShowStandings()
        {
            latestTotalPointsSummary = messageFactory.TotalPointsSummary();
            if (game.AllRoundsHaveBeenPlayed())
            {
                mainView.ShowModal(messageFactory.FinalResults());
                Console.ReadKey(true);
                QuitGame();
            }
            else
            {
                mainView.ShowModalAndHideAfterKeyPress(messageFactory.Standings());
            }
        }

        private void ShowFinalResults()
        {
            if (game.Round > 1)
            {
                mainView.ShowModal(messageFactory.Standings(latestTotalPointsSummary));
            }
        }

        private void ShowRoundResultsModal()
        {
            var message = game.IsTie() ? messageFactory.ItsATie() : messageFactory.RoundResults();
            mainView.ShowModalAndHideAfterKeyPress(message);
        }

        private Task RenderChanges()
        {
            if (renderEventsTask is null || renderEventsTask.IsCompleted)
            {
                renderEventsTask = Task.Run(() => mainView.Render(game.EventBus.GetEvents()));
            }

            return renderEventsTask;
        }

        private void QuitGameIfEscPressed()
        {
            if (!KeyboardPlayerIsAlive() && EscIsPressed())
            {
                if (game.TestMode)
                {
                    mainView.HideModal();
                    QuitGame();
                }
                else
                {
                    Task.WaitAll(renderEventsTask);
                    game.StopStopwatch();
                    mainView.ShowModal(messageFactory.AreYouSureYouWantToQuit());
                    var key = Console.ReadKey(true);
                    mainView.HideModal();

                    if (key.Key == ConsoleKey.Escape)
                    {
                        if (game.Round > 1)
                        {
                            ShowFinalResults();
                        }
                        else
                        {
                            mainView.ShowModal(messageFactory.ByeBye());
                        }
                        QuitGame();
                    }
                    else
                    {
                        game.StartStopwatch();
                    }
                }
            }
        }

        private bool EscIsPressed() => Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape;

        private bool KeyboardPlayerIsAlive()
        {
            return game.Map.Snakes.Any(snake => snake.Player is KeyboardPlayer && snake.IsAlive);
        }

        public static void QuitGame()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Environment.Exit(0);
        }
    }
}
