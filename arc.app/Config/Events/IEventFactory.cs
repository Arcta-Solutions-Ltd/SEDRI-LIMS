using arc.domain.Configuration.EventsConfig;

namespace arc.app.Config.Events
{
    public interface IEventFactory
    {
        EventConfig GetEvent(string eventName);
    }
}
