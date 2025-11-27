using static SitoDeiSiti.Backend.Services.EventServiceManager;

namespace SitoDeiSiti.Backend.Interfaces
{
    public interface IEventServicemanager
    {
        public bool AddEvent<T>(T eventType);
        public bool ReadEvent();
    }
}
