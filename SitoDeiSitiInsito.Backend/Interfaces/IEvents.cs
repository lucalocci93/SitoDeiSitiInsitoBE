using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace SitoDeiSiti.Backend.Interfaces
{
    public interface IEvents
    {
        public Task<Response<List<Events>>> GetEventi();
        public Task<Response<Events>> CreateEvent(Events events);
        public Task<Response<Events>> GetEvent(Guid Id);
        public Task<Response<Events>> UpdateEvent(Events evento);

        public Task<Response<EventSubscription>> SubscribeEvent(EventSubscription subscribeEvent);
        public Task<Response<List<SingleEventSubscription>>> GetEventSubscription(Guid UserId);
        public Task<Response<SingleEventSubscription>> DeleteEventSubscription(EventSubscription subscription);
        public Task<Response<List<Competitor>>> GetCompetitors(Guid EventId);
        public Task<Response<DocumentExt>> CreateCompetitorExcel(Guid EventId);

        public Task<Response<Competition>> AddCompetition (Competition competition);
        public Task<Response<Competition>> DeleteCompetition(Guid CompetitionId, Guid EventId);
        public Task<Response<List<Competition>>> GetCompetitionsByEventAndUser(Guid EventId, Guid UserId);
        public Task<Response<List<Competition>>> GetCompetitionByEvent(Guid EventId);
        public Task<Response<DocumentExt>> GetCompetitionSubscriptionReportByUser(Guid EventId, Guid UserId);
        public Task<Response<DocumentExt>> GetCompetitionSubscriptionReportByMaster(Guid EventId, Guid Org);


        public Task<Response<List<Category>>> GetCategories();
    }
}
