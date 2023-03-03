using Domain.Event;

namespace Domain.Map
{
    public class Food : IGameObject
    {
        public Location Location { get; }
        private const int MaxLifetime = 320; // About 20 s when delay between moves is set to 50 ms
        public int TimeLeft { get; private set; } = MaxLifetime;
        public bool IsExpired => TimeLeft <= 0;
        public bool IsAboutToExpire => TimeLeft <= 50;
        private readonly IEventBus eventBus;

        public Food(Location location, IEventBus eventBus)
        {
            Location = location;
            this.eventBus = eventBus;
        }

        public void ReduceTimeToExpiration()
        {
            if (TimeLeft > 0)
            {
                TimeLeft--;
            }
            if (IsAboutToExpire)
            {
                eventBus.Send(FoodIsAboutToExpire.Instance);
            }
        }
    }
}
