namespace ConsoleApp.Views
{
    class Size
    {
        public int Width { get; }
        public int Height { get; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Size Of(int width, int height)
        {
            return new Size(width, height);
        }
    }
}
