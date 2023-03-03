namespace Domain.Event
{
    public class AmountOfFoodEatenChanged : IEvent
    {
        public Snake.Snake Snake { get; }

        public AmountOfFoodEatenChanged(Snake.Snake snake)
        {
            Snake = snake;
        }
    }
}