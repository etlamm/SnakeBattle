using Domain;
using Domain.Event;
using Domain.Map;
using Domain.Snake;
using Microsoft.Extensions.DependencyInjection;

namespace Dependencies
{
    public static class DependencyInjection
    {
        private static readonly ServiceProvider serviceProvider;

        static DependencyInjection()
        {
            var collection = new ServiceCollection();
            AddDomainServices(collection);
            serviceProvider = collection.BuildServiceProvider();
        }

        private static void AddDomainServices(ServiceCollection collection)
        {
            collection.AddSingleton<GameOptions>();
            collection.AddSingleton<IMap, Map>();
            collection.AddSingleton<IEventBus, EventBus>();
            collection.AddSingleton<ISnakeFactory, SnakeFactory>();
            collection.AddSingleton<Game>();
        }

        public static T GetObject<T>() => serviceProvider.GetService<T>();
    }
}