namespace Domain.Event
{
    public class FoodIsAboutToExpire : IEvent
    {
        public static FoodIsAboutToExpire Instance { get; } = new();

        private FoodIsAboutToExpire()
        {
        }
    }
}
