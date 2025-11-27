using AutoMapper;
using DAL.Enums;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.External.SumUp;
using SitoDeiSiti.Utils.HTTPHandlers.Model;
using SitoDeiSiti.External.SumUp.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using SitoDeiSiti.Backend.External.SumUp.Models.SumUp;
using SitoDeiSiti.Backend.Interfaces;

namespace SitoDeiSiti.Backend.Services
{
    public class AbbonamentoManager : BaseManager, ISubscription
    {
        private readonly IDalAbbonamenti dalAbbonamenti;
        private readonly IDalUtente dalUtente;

        private readonly SumUpManager sumUpManager;

        public AbbonamentoManager(SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache,
            SumUpManager _sumUpManager)
            : base(mapper, hybridCache)
        {
            dalAbbonamenti = new DalAbbonamenti(context);
            dalUtente = new DalUtenti(context);
            sumUpManager = _sumUpManager;
        }

        private enum CacheKey
        {
            SubscriptionType,
            GetUserSubscription,
            GetAllSubscription,
        }

        public async Task<Response<Subscription>> AddAbbonamentoUser(Subscription subscription)
        {
            int AddRow = 0;
            try
            {
                User utente = Mapper.Map<Utente, User>(await dalUtente.GetUtente(subscription.Utente).ConfigureAwait(false));

                //if (string.IsNullOrEmpty(subscription.UrlPagamento))
                //{
                //    if (!await ManagePayment(subscription, utente))
                //    {
                //        return new Response<Subscription>(false, new Error("Errore generazione Url Pagamento"));
                //    }
                //}

                //IReadOnlyCollection<SubscriptionType> TipiAbbonamento = (await GetTipiAbbonamento().ConfigureAwait(false)).Data;

                //subscription.DataScadenza = GetDataScadenzaAbbonamento(TipiAbbonamento.FirstOrDefault(t => t.Id == subscription.IdTipoAbbonamento), subscription.DataIscrizione);

                Abbonamento abbonamento = Mapper.Map<Subscription, Abbonamento>(subscription);

                AddRow = await dalAbbonamenti.AddAbbonamenti(abbonamento).ConfigureAwait(false);

                if (AddRow > 0)
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetUserSubscription), '_', utente.RowGuid)).ConfigureAwait(false);
                    await HybridCache.RemoveAsync(nameof(CacheKey.GetAllSubscription)).ConfigureAwait(false);

                    return new Response<Subscription>(true, subscription);
                }
                else
                {
                    return new Response<Subscription>(false, subscription);
                }
            }
            catch (Exception ex)
            {
                return new Response<Subscription>(false, new Error(ex.Message));
            }
        }

        private DateOnly? GetDataScadenzaAbbonamento(SubscriptionType? subscriptionType, DateOnly dataIscrizione)
        {
            DateOnly? DataScadenza = null;

            if (subscriptionType == null)
                throw new ArgumentNullException("Tipo Abbonamento non valido");

            if (subscriptionType.ScadGiornaliera)
            {
                if (!subscriptionType.GiorniDurata.HasValue)
                {
                    DataScadenza = dataIscrizione.AddDays(1);
                }
            }

            if (subscriptionType.ScadSettimanale)
            {
                if (!subscriptionType.GiorniDurata.HasValue)
                {
                    DataScadenza = dataIscrizione.AddDays(DayOfWeek.Sunday - dataIscrizione.DayOfWeek);
                }
            }

            if (subscriptionType.ScadMensile)
            {
                if (!subscriptionType.GiorniDurata.HasValue)
                {
                    DataScadenza = DateOnly.Parse(new DateTime(dataIscrizione.Year, dataIscrizione.Month, DateTime.DaysInMonth(dataIscrizione.Year, dataIscrizione.Month)).ToShortDateString());
                }
            }

            if (!subscriptionType.ScadMensile && !subscriptionType.ScadSettimanale &&
                subscriptionType.ScadGiornaliera && subscriptionType.GiorniDurata.HasValue)
            {
                DataScadenza = dataIscrizione.AddDays(subscriptionType.GiorniDurata.Value);
            }

            return DataScadenza;
        }

        public async Task<Response<SubscriptionType>> AddTipoAbbonamento(SubscriptionType subscriptionType)
        {
            int RowInserted = 0;

            try
            {
                TipoAbbonamento tipoAbbonamento = Mapper.Map<SubscriptionType, TipoAbbonamento>(subscriptionType);

                RowInserted = await dalAbbonamenti.AddTipoAbbonamento(tipoAbbonamento).ConfigureAwait(false);

                if (RowInserted > 0)
                {
                    await HybridCache.RemoveAsync(nameof(CacheKey.SubscriptionType)).ConfigureAwait(false);

                    return new Response<SubscriptionType>(true, subscriptionType);
                }
                else
                {
                    return new Response<SubscriptionType>(false, subscriptionType);
                }
            }
            catch (Exception ex)
            {
                return new Response<SubscriptionType>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<IReadOnlyCollection<Subscription>>> GetAbbonamentiByUser(Guid utente)
        {
            List<Abbonamento> ListaAbbonamenti = new List<Abbonamento>();
            IReadOnlyCollection<Subscription> SubscriptionList = new List<Subscription>();
            try
            {
                ListaAbbonamenti = await dalAbbonamenti.GetAbbonamentiUtente(utente).ConfigureAwait(false);
                //ListaAbbonamenti = await HybridCache.GetOrCreateAsync(string.Concat(nameof(CacheKey.GetUserSubscription),'_',utente), async result => await dalAbbonamenti.GetAbbonamentiUtente(utente).ConfigureAwait(false)).ConfigureAwait(false);

                foreach (var sub in ListaAbbonamenti.Where(a => a.Pagato.HasValue && a.Pagato.Value && (!a.Attivo.HasValue || !a.Attivo.Value)))
                {
                    if (sub.DataIscrizione.HasValue && sub.DataIscrizione.Value <= DateTime.Now
                    && (!sub.DataScadenza.HasValue || (sub.DataScadenza.HasValue && sub.DataScadenza >= DateTime.Now)))
                    {
                        sub.Attivo = true;
                        await dalAbbonamenti.UpdateAbbonamento(DbOperationsAbbonamentoEnums.AggiornaStatoAbbonamento, sub, sub.TipoAbbonamentoNavigation).ConfigureAwait(false);
                    }
                }

                foreach(var sub in ListaAbbonamenti.Where(a => (a.Pagato.HasValue && !a.Pagato.Value)))
                {
                    if(sub.DataScadenza.HasValue && sub.DataScadenza >= DateTime.Now)
                    {
                        User user = Mapper.Map<Utente, User>(await dalUtente.GetUtente(sub.Utente).ConfigureAwait(false));

                        sub.IdCheckout = Guid.NewGuid().ToString();
                        sub.UrlPagamento = await GetPaymentUrl(sub, user).ConfigureAwait(false);
                        await dalAbbonamenti.UpdateAbbonamento(DbOperationsAbbonamentoEnums.AggiornaInfoPagamento, sub, sub.TipoAbbonamentoNavigation).ConfigureAwait(false);
                    }
                }

                SubscriptionList = Mapper.Map<List<Abbonamento>, IReadOnlyCollection<Subscription>>(ListaAbbonamenti);

                return new Response<IReadOnlyCollection<Subscription>>(true, SubscriptionList);
            }
            catch (Exception ex)
            {
                return new Response<IReadOnlyCollection<Subscription>>(false, SubscriptionList);
            }
        }

        public async Task<Response<Subscription>> GetAbbonamento(Guid utente, int Id)
        {
            Subscription subscription = new Subscription();

            try
            {
                Abbonamento? abbonamento = await dalAbbonamenti.GetAbbonamento(utente, Id).ConfigureAwait(false);

                if (abbonamento != null)
                {
                    subscription = Mapper.Map<Abbonamento, Subscription>(abbonamento);

                    return new Response<Subscription>(true, subscription);
                }
                else
                {
                    return new Response<Subscription>(false, subscription);

                }
            }
            catch (Exception ex)
            {
                return new Response<Subscription>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<IReadOnlyCollection<Subscription>>> GetAllAbbonamenti()
        {
            IReadOnlyCollection<Subscription> SubscriptionList = new List<Subscription>();
            List<Abbonamento> AbbonamentoList = new();

            try
            {
                //AbbonamentoList = await dalAbbonamenti.GetAllAbbonamenti().ConfigureAwait(false);
                //SubscriptionList = Mapper.Map<List<Abbonamento>, List<Subscription>>(AbbonamentoList);

                SubscriptionList = await HybridCache.GetOrCreateAsync(nameof(CacheKey.GetAllSubscription), async result => Mapper.Map<IReadOnlyCollection<Subscription>>(await dalAbbonamenti.GetAllAbbonamenti().ConfigureAwait(false))).ConfigureAwait(false);

                return new Response<IReadOnlyCollection<Subscription>>(true, SubscriptionList);
            }
            catch (Exception ex)
            {
                return new Response<IReadOnlyCollection<Subscription>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<IReadOnlyCollection<SubscriptionType>>> GetTipiAbbonamento()
        {
            IReadOnlyCollection<SubscriptionType> subscriptionTypes = new List<SubscriptionType>();

            try
            {
                subscriptionTypes = await HybridCache.GetOrCreateAsync(nameof(CacheKey.SubscriptionType), async result => Mapper.Map<IReadOnlyCollection<SubscriptionType>>(await dalAbbonamenti.GetTipiAbbonamento().ConfigureAwait(false))).ConfigureAwait(false);
            
                if(subscriptionTypes != null && subscriptionTypes.Any())
                {
                    return new Response<IReadOnlyCollection<SubscriptionType>>(true, subscriptionTypes);
                }
                else
                {
                    return new Response<IReadOnlyCollection<SubscriptionType>>(false, new Error("Nessun tipo abbonamento trovato"));
                }
            }
            catch (Exception ex)
            {
                return new Response<IReadOnlyCollection<SubscriptionType>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Subscription>> UpdateAbbonamentoUser(DbOperationsAbbonamentoEnums operation, Subscription subscription)
        {
            int RowUpdated = 0;
            try
            {
                Abbonamento abbonamento = Mapper.Map<Subscription, Abbonamento>(subscription);
                TipoAbbonamento tipoAbbonamento = Mapper.Map<Subscription, TipoAbbonamento>(subscription);

                RowUpdated = await dalAbbonamenti.UpdateAbbonamento(operation, abbonamento, tipoAbbonamento).ConfigureAwait(false);

                await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetUserSubscription), '_', subscription.Utente)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllSubscription)).ConfigureAwait(false);

                //TODO invio mail in caso di notifica pagamento

                return new Response<Subscription>(true, subscription);
            }
            catch (Exception ex)
            {
                return new Response<Subscription>(false, new Error(ex.Message));
            }
        }

        private async Task<bool> ManagePayment(Subscription subscription, SitoDeiSiti.DTOs.User utente)
        {
            try
            {
                subscription.IdCheckout = Guid.NewGuid().ToString();

                HostedCheckoutInput input = new HostedCheckoutInput()
                {
                    amount = subscription.Importo.HasValue ? (decimal)subscription.Importo : 0,
                    currency = "EUR",
                    checkout_reference = subscription.IdCheckout,
                    description = $"Pagamento Abbonamento {subscription.TipoAbbonamento} {utente.Nome} {utente.Cognome}",
                    //merchant_code = SumUpOptions.Value.MerchantCode
                };

                HostedCheckoutOutput checkoutOutput = await sumUpManager.CreateHostedCheckout(input).ConfigureAwait(false);

                if(checkoutOutput is not null)
                {
                    subscription.UrlPagamento = checkoutOutput.hosted_checkout_url;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetPaymentUrl(Abbonamento subscription, User utente)
        {
            try
            {
                HostedCheckoutInput input = new HostedCheckoutInput()
                {
                    amount = subscription.Importo.HasValue ? (decimal)subscription.Importo : 0,
                    currency = "EUR",
                    checkout_reference = subscription.IdCheckout,
                    description = $"Pagamento Abbonamento {subscription.TipoAbbonamentoNavigation.Descrizione} {utente.Nome} {utente.Cognome} {utente.CodFiscale.ToUpper()}",
                    //merchant_code = SumUpOptions.Value.MerchantCode
                };

                HostedCheckoutOutput checkoutOutput = await sumUpManager.CreateHostedCheckout(input).ConfigureAwait(false);

                if (checkoutOutput is not null)
                {
                    return checkoutOutput.hosted_checkout_url;
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
