namespace Domain.Event
{
    public class NewRoundStarted : IEvent
    {
        public static NewRoundStarted Instance { get; } = new();
        private NewRoundStarted()
        {
        }
    }
}