namespace Domain.Snake
{
    public class SnakePart
    {
        public Location Location { get; set; }

        public SnakePart(Location location)
        {
            Location = location;
        }
    }
}