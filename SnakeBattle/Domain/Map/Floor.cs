namespace Domain.Map
{
    public class Floor : IGameObject
    {
        public static Floor Instance { get; } = new();

        private Floor()
        {
        }
    }
}