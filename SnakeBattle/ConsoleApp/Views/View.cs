namespace ConsoleApp.Views
{
    abstract class View
    {
        /// <summary>
        /// The absolute position of the view in the console.
        /// </summary>
        public Position Position { get; }
        public Size Size { get; }
        public Area Area => new(Position, Size);

        protected View(Position position, Size size)
        {
            Position = position;
            Size = size;
        }

        public abstract void Render();

        public virtual void Render(Area area)
        {
            if (AreaIsInsideView(area))
            {
                Render();
            }
        }

        protected bool AreaIsInsideView(Area area)
        {
            return PositionIsInsideView(area.UpperLeftCorner) || PositionIsInsideView(area.LowerRightCorner);
        }

        protected bool AreaIsOutsideView(Area area)
        {
            return !AreaIsInsideView(area);
        }

        protected bool PositionIsInsideView(Position position)
        {
            return position.X >= Position.X
                   && position.X < Position.X + Size.Width
                   && position.Y >= Position.Y
                   && position.Y < Position.Y + Size.Height;
        }

        protected bool PositionIsOutsideView(Position position)
        {
            return !PositionIsInsideView(position);
        }
    }
}
