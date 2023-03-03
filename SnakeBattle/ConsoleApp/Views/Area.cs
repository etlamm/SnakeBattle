namespace ConsoleApp.Views
{
    class Area
    {
        public Position Position { get; }
        public Size Size { get; }
        public Position UpperLeftCorner => Position;
        public Position LowerRightCorner => Position.At(Position.X + Size.Width - 1, Position.Y + Size.Height - 1);

        public Area(Position position, Size size)
        {
            Position = position;
            Size = size;
        }
    }
}
