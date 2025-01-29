using DAL.Enums;
using Identity.DTOs;
using Identity.Models;

namespace Identity.Interfaces
{
    public interface ISubscription
    {
        public Task<Response<List<SubscriptionType>>> GetTipiAbbonamento();
        public Task<Response<SubscriptionType>> AddTipoAbbonamento(SubscriptionType subscriptionType);
        public Task<Response<List<Subscription>>> GetAllAbbonamenti();
        public Task<Response<List<Subscription>>> GetAbbonamentiByUser(Guid Utente);
        public Task<Response<Subscription>> GetAbbonamento(Guid utente, int Id);
        public Task<Response<Subscription>> AddAbbonamentoUser(Subscription subscription);
        public Task<Response<Subscription>> UpdateAbbonamentoUser(DbOperationsAbbonamentoEnums operation, Guid Utente, Subscription subscription);
    }
}
