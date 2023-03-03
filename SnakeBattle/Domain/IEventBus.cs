using System.Collections.Generic;
using Domain.Event;

namespace Domain
{
    public interface IEventBus
    {
        void Send(IEvent e);
        List<IEvent> GetEvents();
    }
}