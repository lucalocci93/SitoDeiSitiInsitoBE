using Identity.DTOs;
using Identity.Models;
using SitoDeiSitiService.DTOs;

namespace SitoDeiSiti.Interfaces
{
    public interface IEvents
    {
        public Task<Response<List<Events>>> GetEventi();
        public Task<Response<Events>> CreateEvent(Events events);
        public Task<Response<Events>> GetEvent(Guid Id);
        public Task<Response<Events>> UpdateEvent(Events evento);
        public Task<Response<EventSubscription>> SubscribeEvent(EventSubscription subscribeEvent);
        public Task<Response<List<SingleEventSubscription>>> GetEventSubscription(Guid UserId);
        public Task<Response<SingleEventSubscription>> DeleteEventSubscription(Guid EventId, Guid UserId, int Category);
        public Task<Response<List<Competitor>>> GetCompetitors(Guid EventId);
        public Task<Response<DocumentExt>> CreateCompetitorExcel(Guid EventId);


        public Task<Response<List<Category>>> GetCategories();
    }
}
