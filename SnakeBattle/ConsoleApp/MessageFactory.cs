using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Player;
using Domain.Snake;

namespace ConsoleApp
{
    class MessageFactory
    {
        private readonly Game game;
        private string RoundResultsHeading => $"Round {game.Round} results";
        private const string RunningInSnakeTestingMode = "Running in snake testing mode";
        private const string PressAnyKeyToContinue = "Press any key to continue";
        private const string PressAnyKeyToQuit = "Press any key to quit";

        public MessageFactory(Game game)
        {
            this.game = game;
        }

        public List<string> StartingSnakeBattle()
        {
            var message =  new List<string>
            {
                "Snake battle!",
                "",
                "",
                "Players:",
                ""
            };
            message.AddRange(game.Map.Snakes.Select(snake => $"{GetTeamMembersAsText(snake.Player)} ({snake.Player.GetType().Name})"));
            message.AddRange(new List<string>
            {
                "",
                PressAnyKeyToContinue
            });

            return message;
        }

        public List<string> ItsATie()
        {
            var winners = game.GetWinnersInATie();

            var message = new List<string>
            {
                RoundResultsHeading,
                "",
                "",
                "It's a tie!",
                "",
                "Round winners:"
            };
            message.AddRange(winners.Select(snake => string.Join(", ", snake.Player.TeamMembers)));
            message.AddRange(new List<string>
            {
                "",
                $"Points: {winners[0].PointsInCurrentRound}",
                "",
                PressAnyKeyToContinue
            });

            return message;
        }

        private string GetTimeLivedString(ISnake snake) => SecondsToTime(snake.SecondsLived);

        private string SecondsToTime(int seconds) =>
            (seconds >= 60 ? $"{seconds / 60} min " : "") + $"{seconds % 60} s";

        public List<string> RoundResults()
        {
            var winner = game.GetWinner();
            var timeLivedString = GetTimeLivedString(winner);
            return new List<string>
            {
                RoundResultsHeading,
                "",
                "",
                "Round winner:",
                GetTeamMembersAsText(winner.Player),
                "",
                $"Points: {winner.PointsInCurrentRound}",
                "",
                $"Time lived: {timeLivedString}",
                "",
                PressAnyKeyToContinue
            };
        }

        private static string GetTeamMembersAsText(IPlayer player)
        {
            return string.Join(", ", player.TeamMembers);
        }

        public IEnumerable<string> DidNotFindCustomPlayersForTestMode() => new[]
        {
            RunningInSnakeTestingMode,
            "",
            "",
            $"Didn't find any custom player implementations.",
            $"Follow the instructions in {nameof(PlayerTemplate)} class",
            "to create a custom player.",
            "",
            PressAnyKeyToQuit
        };

        public IEnumerable<string> FoundOneCustomPlayer(Type customPlayerType) => new[]
        {
            RunningInSnakeTestingMode,
            "",
            "",
            $"Found a custom player implementation:",
            $"{customPlayerType.Name}",
            "",
            $"Starting the game with two players:",
            $"{customPlayerType.Name} and a keyboard-controlled player.",
            "Use the keyboard-controlled player",
            "to test how well your custom player performs.",
            "",
            $"To play with multiple instances of {customPlayerType.Name},",
            "type the number of instances to add (2-9)."
        };

        public IEnumerable<string> FoundMultipleCustomPlayers(List<Type> customPlayers)
        {
            var message = new[]
            {
                RunningInSnakeTestingMode,
                "",
                "",
                $"Found {customPlayers.Count} custom player implementations:"
            }.ToList();
            foreach (var customPlayer in customPlayers)
            {
                message.Add(customPlayer.Name);
            }
            message.AddRange(new string[]
            {
                "",
                $"Starting the game with the custom players",
                $"and a keyboard-controlled player.",
                "Use the keyboard-controlled player",
                "to test how well your custom players perform.",
                "",
                PressAnyKeyToContinue
            });

            return message;
        }

        public List<string> Standings()
        {
            var message = new List<string>
            {
                $"Standings after {game.NumberOfFullRoundsPlayed} {(game.NumberOfFullRoundsPlayed == 1 ? "round" : "rounds")}",
                "",
                ""
            };
            message.AddRange(TotalPointsSummary());
            message.AddRange(new List<string>
            {
                "",
                "Press any key to continue"
            });

            return message;
        }

        public List<string> FinalResults()
        {
            var message = new List<string>
            {
                $"Final results",
                "",
                ""
            };
            message.AddRange(TotalPointsSummary());
            message.AddRange(new List<string>
            {
                "",
                "Press any key to quit"
            });

            return message;
        }

        public List<string> TotalPointsSummary()
        {
            var message = new List<string>();

            var orderedSnakes = game.Map.Snakes.OrderByDescending(snake => snake.TotalPoints).ToList();
            var results = new List<Tuple<string, string>>();
            foreach (var snake in orderedSnakes)
            {
                results.Add(new Tuple<string, string>(
                    $"{GetTeamMembersAsText(snake.Player)} ",
                    snake.TotalPoints.ToString()));
            }

            var maxTeamMembersTextWidth = results.Max(element => element.Item1.Length);
            var maxTotalPointsWidth = results.Max(element => element.Item2.Length);
            foreach (var element in results)
            {
                message.Add(element.Item1.PadRight(maxTeamMembersTextWidth)
                            + element.Item2.PadLeft(maxTotalPointsWidth));
            }

            message.AddRange(new List<string>
            {
                "",
                $"Total time played: {SecondsToTime(game.TotalSecondsPlayed)}"
            });

            return message;
        }

        public IEnumerable<string> RoundStart()
        {
            return new List<string>
            {
                $"Round {game.Round}"
            };
        }

        public IEnumerable<string> DidNotFindCustomPlayersForBattleMode() => new[]
        {
            "Didn't find any custom player implementations",
            "",
            "",
            $"Follow the instructions in {nameof(PlayerTemplate)} class",
            "to create a custom player.",
            "",
            PressAnyKeyToQuit
        };

        public IEnumerable<string> AreYouSureYouWantToQuit()
        {
            return new List<string>
            {
                "Are you sure you want to quit?",
                "",
                "",
                "Press ESC to quit or any other key to continue",
            };
        }

        public List<string> Standings(List<string> totalPointsSummary)
        {
            var message = new List<string>
            {
                $"Standings after {game.NumberOfFullRoundsPlayed} rounds",
                "",
                ""
            };
            message.AddRange(totalPointsSummary);
            return message;
        }

        public IEnumerable<string> ByeBye() => new List<string> { "Bye bye!" };

        public IEnumerable<string> FinalRound() => new[] { "Final round" };

        public IEnumerable<string> TimeIsUp() => new[]
        {
            "Time's up",
            "",
            "",
            "No-one ate food in too long time.",
            "",
            PressAnyKeyToContinue
        };
    }
}
