using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp;
using ConsoleApp.Views;
using Dependencies;
using Domain;

namespace EndToEndTests
{
    class Program
    {
        private static Game game;
        private static MainView mainView;
        private static List<IEndToEndTest> tests;

        static async Task Main(string[] args)
        {
            game = DependencyInjection.GetObject<Game>();
            mainView = new MainView(game);

            ShowOopsYouProbablyShouldNotBeHereModal();
            ShowTestListModal();
            await SelectTestToRun();
        }

        private static async Task SelectTestToRun()
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                GameRunner.QuitGame();
            }

            mainView.HideModal();

            if (int.TryParse(key.KeyChar.ToString(), out var testNumber) && testNumber <= tests.Count)
            {
                await tests[testNumber - 1].Run();
            }
            else
            {
                GameRunner.QuitGame();
            }
        }

        private static void ShowTestListModal()
        {
            var message = new List<string>
            {
                "End-to-end tests",
                "",
                "",
                "Select a test to run:",
                ""
            };

            tests = GetEndToEndTests();

            var testList = new List<string>();
            for (var i = 0; i < tests.Count; i++)
            {
                testList.Add($"{i + 1}: {tests[i].Category} - {tests[i].Description}");
            }

            var lengthOfLongestLineInTestList = testList.Max(line => line.Length);
            for (int i = 0; i < testList.Count; i++)
            {
                testList[i] = testList[i].PadRight(lengthOfLongestLineInTestList);
            }

            message.AddRange(testList);
            mainView.ShowModal(message.ToArray());
        }

        private static void ShowOopsYouProbablyShouldNotBeHereModal()
        {
            mainView.ShowModalAndHideAfterKeyPress(new[]
            {
                "Oops... you probably shouldn't be here",
                "",
                "",
                "Set ConsoleApp as the startup project to run the game."
            });
        }

        private static List<IEndToEndTest> GetEndToEndTests()
        {
            var testClasses = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IEndToEndTest).IsAssignableFrom(type) && type != typeof(IEndToEndTest))
                .ToList();

            return testClasses.Select(testClass => (IEndToEndTest)Activator.CreateInstance(testClass))
                .OrderBy(test => test.Category)
                .ThenBy(test => test.Description)
                .ToList();
        }
    }
}
