using DAL.Enums;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace SitoDeiSiti.Backend.Interfaces
{
    public interface ISubscription
    {
        public Task<Response<IReadOnlyCollection<SubscriptionType>>> GetTipiAbbonamento();
        public Task<Response<SubscriptionType>> AddTipoAbbonamento(SubscriptionType subscriptionType);
        public Task<Response<IReadOnlyCollection<Subscription>>> GetAllAbbonamenti();
        public Task<Response<IReadOnlyCollection<Subscription>>> GetAbbonamentiByUser(Guid Utente);
        public Task<Response<Subscription>> GetAbbonamento(Guid utente, int Id);
        public Task<Response<Subscription>> AddAbbonamentoUser(Subscription subscription);
        public Task<Response<Subscription>> UpdateAbbonamentoUser(DbOperationsAbbonamentoEnums operation, Subscription subscription);
    }
}
