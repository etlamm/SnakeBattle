namespace Domain.Event
{
    public class StopwatchUpdated : IEvent
    {
        public static StopwatchUpdated Instance { get; } = new();

        private StopwatchUpdated()
        {
        }
    }
}