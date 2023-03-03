namespace Domain.Map
{
    public class Wall : IGameObject
    {
        public static Wall Instance { get; } = new Wall();

        private Wall()
        {
        }
    }
}