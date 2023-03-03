using System.Collections.Generic;
using System.Linq;

namespace Domain.Event
{
    public class EventBus : IEventBus
    {
        private readonly List<IEvent> events = new();

        public void Send(IEvent e)
        {
            lock (events)
            {
                events.Add(e);
            }
        }

        public List<IEvent> GetEvents()
        {
            List<IEvent> result;

            lock (events)
            {
                result = events.ToList();
                events.Clear();
            }

            return result;
        }
    }
}
