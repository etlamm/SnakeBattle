# Snake battle

Snake battle is an exercise for practicing teamwork (and coding). Your task is to develop a CPU player that controls a snake in a classic snake game. The snake has two goals:

1. Eat as much food as possible
1. Stay alive by avoiding obstacles

The repository contains a full snake game, but the snake is just a dummy. Your job is to make it smart.

The game is a C# console app (.NET 6).

## How is the exercise organized?

1. The participants are divided into teams of 2-3 persons.
1. Each team gathers to implement their snake collaboratively, using one computer per team.
1. In the end, all the snakes are placed on the same map, and a set of rounds is played. The team whose snake eats most food in total wins.

## Rules for the final battle

- We’ll play 3 rounds.
- A round ends when all snakes have died (or if 1 minute passes without any snake eating food).
- The snake who eats most food in total during all the rounds wins.

## How to get started with coding?

1. Clone this repository.
1. Open the solution in Visual Studio.
1. Create a branch for your team.
1. Duplicate file Domain/Player/PlayerTemplate.cs and rename the copy (both the class and the file) as you like. The game will automatically detect the new class and use it to control the snake.
1. Run the app to see how the game looks.
1. Start modifying your player class by following the instructions in the file.
1. When the time is up, commit and push your player file. Do not commit any other changes.
1. Create a pull request for merging your branch into the main branch. Add the facilitator of the event as the reviewer.

## Tips

1. Start simple.
1. Don’t aim for too advanced solutions. We don’t have time for that.
1. If your snake doesn’t work as expected, add a breakpoint in your player class and start debugging.
1. Note that if your player throws an unhandled exception, the game won’t crash, but your snake will just keep going forward silently.
1. Contact the facilitator if you have technical issues.

## Running the final battle (info for facilitator)

1. Complete the pull requests created by the teams and make sure all the newly created player classes are placed in Domain/Player/ folder.
1. In ConsoleApp/config.json, set "TestMode" to false.
1. Run the solution.
