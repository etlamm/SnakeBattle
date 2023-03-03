using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleApp.Views;
using Domain;
using Domain.Player;

namespace ConsoleApp
{
    class PlayerAdder
    {
        private Game game;
        private MainView mainView;
        private GameOptions options;
        private MessageFactory messageFactory;

        public PlayerAdder(Game game, MainView mainView, GameOptions options)
        {
            this.game = game;
            this.mainView = mainView;
            this.options = options;
            this.messageFactory = new MessageFactory(game);
        }

        public void AddPlayers()
        {
            if (options.TestMode)
            {
                AddPlayersForTestMode();
            }
            else
            {
                AddPlayersForBattleMode();
            }
        }

        private void AddPlayersForTestMode()
        {
            var customPlayers = GetCustomPlayerImplementations();

            if (customPlayers.Count == 0)
            {
                NoCustomPlayersFoundForTestMode();
            }
            else if (customPlayers.Count == 1)
            {
                OneCustomPlayerFoundForTestMode(customPlayers.Single());
            }
            else
            {
                MultipleCustomPlayersFoundForTestMode(customPlayers);
            }
        }

        private List<Type> GetCustomPlayerImplementations()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IPlayer).IsAssignableFrom(type) && type != typeof(IPlayer) && type != typeof(PlayerTemplate) && type != typeof(KeyboardPlayer))
                .ToList();
        }

        private void NoCustomPlayersFoundForTestMode()
        {
            mainView.ShowModal(messageFactory.DidNotFindCustomPlayersForTestMode());
            Console.ReadKey(true);
            GameRunner.QuitGame();
        }

        private void OneCustomPlayerFoundForTestMode(Type customPlayerType)
        {
            mainView.ShowModal(messageFactory.FoundOneCustomPlayer(customPlayerType));
            var key = Console.ReadKey(true);
            var numberOfCustomPlayerInstances = new Regex("[1-9]").IsMatch(key.KeyChar.ToString())
                ? int.Parse(key.KeyChar.ToString())
                : 1;
            mainView.HideModal();

            var players = new List<IPlayer>();
            for (int i = 0; i < numberOfCustomPlayerInstances; i++)
            {
                players.Add((IPlayer)Activator.CreateInstance(customPlayerType));
            }

            players.Add(new KeyboardPlayer());
            game.AddPlayers(players.ToArray());
        }

        private void MultipleCustomPlayersFoundForTestMode(List<Type> customPlayers)
        {
            mainView.ShowModalAndHideAfterKeyPress(messageFactory.FoundMultipleCustomPlayers(customPlayers));

            var players = new List<IPlayer>();
            foreach (var customPlayer in customPlayers)
            {
                players.Add((IPlayer)Activator.CreateInstance(customPlayer));
            }

            players.Add(new KeyboardPlayer());
            game.AddPlayers(players.ToArray());
        }

        private void AddPlayersForBattleMode()
        {
            var customPlayers = GetCustomPlayerImplementations();

            if (customPlayers.Count == 0)
            {
                mainView.ShowModal(messageFactory.DidNotFindCustomPlayersForBattleMode());
                Console.ReadKey(true);
                GameRunner.QuitGame();
            }

            var players = new List<IPlayer>();
            foreach (var type in customPlayers)
            {
                players.Add((IPlayer)Activator.CreateInstance(type));
            }
            game.AddPlayers(players.ToArray());
        }
    }
}
