namespace Domain.Event
{
    public class CellContentChanged : IEvent
    {
        public Location Location { get; }

        public CellContentChanged(Location location)
        {
            Location = location;
        }
    }
}