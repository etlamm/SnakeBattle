namespace Domain.Player
{
    public class AddPlayerRequest
    {
        public IPlayer Player { get; set; }
        public Location Location { get; set; }
        public Direction Direction { get; set; }

        public AddPlayerRequest(IPlayer player, Location location, Direction direction)
        {
            Player = player;
            Location = location;
            Direction = direction;
        }
    }
}
