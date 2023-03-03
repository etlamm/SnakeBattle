using System;

namespace Domain
{
    public class GameOptions
    {
        public bool TestMode { get; set; } = false;
        public int MillisecondsBetweenMoves { get; set; } = 50;
        public int MapWidth { get; set; } = 120;
        public int MapHeight { get; set; } = 60;
        public bool EnableFood { get; set; } = true;
        public bool EnableRounds { get; set; } = true;
        public int Rounds { get; set; } = 3;
        public int InitialSnakeLength { get; set; } = 10;
        public int RandomSeed { get; set; } = DateTime.Now.Millisecond;
        public int MaxMovesToWaitAfterFoodEatenBeforeEndingRound = 960; // About 1 min with 50 ms delay between moves
    }
}
