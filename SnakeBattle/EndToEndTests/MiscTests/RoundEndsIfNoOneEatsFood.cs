﻿using System;
using System.Threading.Tasks;
using ConsoleApp;
using ConsoleApp.Views;
using Dependencies;
using Domain;
using EndToEndTests.Player;

namespace EndToEndTests.MiscTests
{
    class RoundEndsIfNoOneEatsFood : IEndToEndTest
    {
        private Game game;
        private MainView mainView;

        public string Category => Categories.MiscTests;

        public string Description => "Round ends if no-one eats food";

        public async Task Run()
        {
            var options = DependencyInjection.GetObject<GameOptions>();
            options.TestMode = false;
            options.MillisecondsBetweenMoves = 15;
            options.EnableFood = false;
            options.MaxMovesToWaitAfterFoodEatenBeforeEndingRound = 10;
            options.Rounds = 2;  
            game = DependencyInjection.GetObject<Game>();

            mainView = new MainView(game);

            game.AddPlayers(new ForwardGoingPlayer(), new ForwardGoingPlayer());

            mainView.Render();

            await new GameRunner(game, mainView).RunGame();
        }
    }
}
