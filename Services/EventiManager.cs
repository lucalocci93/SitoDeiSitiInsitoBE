using AutoMapper;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Caching.Hybrid;
using OfficeOpenXml;
using SitoDeiSiti.Backend.DTOs;
using SitoDeiSiti.Backend.Interfaces;
using SitoDeiSiti.Backend.Utils.PDF;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.Models.ConfigSettings;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Events = SitoDeiSiti.DTOs.Events;

namespace SitoDeiSiti.Backend.Services
{
    public class EventiManager : BaseManager, IEvents
    {
        private readonly IDalEventi dalEventi;
        private readonly IDalUtente dalUtente;
        private readonly IDalSito dalSito;

        public EventiManager(SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache)
            : base(mapper, hybridCache)
        {
            dalEventi = new DalEventi(context);
            dalUtente = new DalUtenti(context);
            dalSito = new DalSito(context);
        }

        private enum CacheKey
        {
            EventCategories,
            GetEvents,
            GetEvent,
            GetCompetitionsByEvent_
        }

        public async Task<Response<Events>> CreateEvent(Events events)
        {
            Evento evento = new();
            bool IsEventCreated = false;
            SequentialGuidValueGenerator generator = new();

            try
            {
                //evento = Mapper.Map<Events, Evento>(events);
                evento.Id = await generator.NextAsync(null).ConfigureAwait(false);
                evento.NomeEvento = events.NomeEvento;
                evento.LuogoEvento = events.LuogoEvento;
                evento.Descrizione = events.Descrizione;
                evento.Link = events.Link;
                evento.DataInizioEvento = events.DataInizioEvento;
                evento.DataFineEvento = events.DataFineEvento;
                evento.Categorie = JsonSerializer.Serialize(events.Categorie);
                evento.Copertina = string.IsNullOrEmpty(events.Copertina.ImageData) ?
                        null : Convert.FromBase64String(events.Copertina.ImageData);
                evento.ImportoIscrizione = events.ImportoIscrizione;
                evento.ChiusuraIscrizioni = events.ChiusuraIscrizioni;

                IsEventCreated = await dalEventi.CreateEvento(evento).ConfigureAwait(false);

                if (IsEventCreated)
                {
                    //rimuovo cache eventi
                    await HybridCache.RemoveAsync(nameof(CacheKey.GetEvents));
                    
                    return new Response<Events>(true, events);
                }
                else
                {
                    throw new Exception("Errore creazione evento");
                }

            }
            catch (Exception ex)
            {
                return new Response<Events>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Category>>> GetCategories()
        {
            List<Category> categories = new();
            List<Categoria> categorie = new();

            try
            {
                categories = await HybridCache.GetOrCreateAsync<List<Category>>(nameof(CacheKey.EventCategories), async result => Mapper.Map<List<Category>>(await dalEventi.GetCategorie().ConfigureAwait(false)));

                if(categories != null && categories.Any())
                {
                    return new Response<List<Category>>(true, categories);
                }
                else
                {
                    return new Response<List<Category>>(false, new Error("Nessuna categoria trovata"));
                }
            }
            catch(Exception ex)
            {
                return new Response<List<Category>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Events>> GetEvent(Guid Id)
        {
            Evento? evento = new Evento();
            Events e = new Events();
            Dictionary<Guid, Category> categories = new Dictionary<Guid, Category>();

            try
            {
                //evento = await dalEventi.GetEvent(Id).ConfigureAwait(false);
                evento = await HybridCache.GetOrCreateAsync<Evento>(string.Concat(nameof(CacheKey.GetEvent),'_',Id), async result => await dalEventi.GetEvent(Id).ConfigureAwait(false));

                if (evento is not null)
                {
                    e.Id = evento.Id;
                    e.NomeEvento = evento.NomeEvento;
                    e.LuogoEvento = evento.LuogoEvento;
                    e.Descrizione = evento.Descrizione;
                    e.Link = evento.Link;

                    e.DataInizioEvento = new DateTime(evento.DataInizioEvento.Year, evento.DataInizioEvento.Month, evento.DataInizioEvento.Day);
                    e.DataFineEvento = new DateTime(evento.DataFineEvento.Year, evento.DataFineEvento.Month, evento.DataFineEvento.Day);

                    e.ImportoIscrizione = evento.ImportoIscrizione;
                    e.ChiusuraIscrizioni = evento.ChiusuraIscrizioni;

                    if (!string.IsNullOrEmpty(evento.Categorie))
                    {
                        List<Category>? categoria = JsonSerializer.Deserialize<List<Category>>(evento.Categorie);

                        if (categoria != null)
                        {
                            foreach (var c in categoria)
                            {
                                e.Categorie.Add(new Category(c.Id, c.Descrizione));
                            }
                        }
                    }

                    e.Copertina = new Copertina()
                    {
                        ImageData = evento.Copertina != null ? Convert.ToBase64String(evento.Copertina) : string.Empty
                    };
                }

                return new Response<Events>(true, e);
            }
            catch (Exception ex)
            {
                return new Response<Events>(false, new Error(ex.Message));
            }
        }


        public async Task<Response<List<Events>>> GetEventi()
        {
            List<Evento> eventi = new List<Evento>();
            List<Events> events = new List<Events>();
            Dictionary<Guid, Category> categories = new Dictionary<Guid, Category>();

            try
            {

                //eventi = await dalEventi.GetEventi().ConfigureAwait(false);
                eventi = await HybridCache.GetOrCreateAsync<List<Evento>>(nameof(CacheKey.GetEvents), async result => await dalEventi.GetEventi().ConfigureAwait(false));

                if (eventi == null || eventi.Count == 0)
                {
                    return new Response<List<Events>>(true, events); //ritorno lista vuota perche è gestita visualizzazione a FE
                }

                foreach (Evento evento in eventi)
                {
                    Events e = new();

                    e.Id = evento.Id;
                    e.NomeEvento = evento.NomeEvento;
                    e.LuogoEvento = evento.LuogoEvento;
                    e.Descrizione = evento.Descrizione;
                    e.Link = evento.Link;

                    e.DataInizioEvento = new DateTime(evento.DataInizioEvento.Year, evento.DataInizioEvento.Month, evento.DataInizioEvento.Day);
                    e.DataFineEvento = new DateTime(evento.DataFineEvento.Year, evento.DataFineEvento.Month, evento.DataFineEvento.Day);

                    e.ImportoIscrizione = evento.ImportoIscrizione;
                    e.ChiusuraIscrizioni = evento.ChiusuraIscrizioni;

                    if (!string.IsNullOrEmpty(evento.Categorie))
                    {
                        List<Category> categoria = JsonSerializer.Deserialize<List<Category>>(evento.Categorie);

                        if (categoria != null)
                        {
                            foreach (var c in categoria)
                            {
                                e.Categorie.Add(new Category(c.Id, c.Descrizione));
                            }
                        }
                    }

                    e.Copertina = new Copertina()
                    {
                        ImageData = evento.Copertina != null ? Convert.ToBase64String(evento.Copertina) : string.Empty
                    };

                    events.Add(e);
                }

                return new Response<List<Events>>(true, events);

            }
            catch (Exception ex)
            {
                return new Response<List<Events>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Events>> UpdateEvent(Events evento)
        {
            Evento? e = new Evento();
            Dictionary<Guid, Category> categories = new Dictionary<Guid, Category>();

            try
            {
                if (evento is not null && evento.Id.HasValue)
                {
                    e.Id = evento.Id.Value;
                    e.NomeEvento = evento.NomeEvento;
                    e.LuogoEvento = evento.LuogoEvento;
                    e.Descrizione = evento.Descrizione;
                    e.Link = evento.Link;

                    e.DataInizioEvento = evento.DataInizioEvento;
                    e.DataFineEvento = evento.DataFineEvento;

                    e.Categorie = JsonSerializer.Serialize(evento.Categorie);

                    e.Copertina = evento.Copertina.ImageData != null ? Convert.FromBase64String(evento.Copertina.ImageData) : Array.Empty<byte>();

                    bool isUpdated = await dalEventi.UpdateEvento(e).ConfigureAwait(false);

                    if (isUpdated)
                    {
                        //rimuovo cache eventi
                        await HybridCache.RemoveAsync(nameof(CacheKey.GetEvents));
                        await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetEvent), '_', e.Id));

                        return new Response<Events>(true, evento);
                    }
                    else
                    {
                        return new Response<Events>(true, new Error("Errore durante aggiornamento evento"));
                    }
                }
                else
                {
                    return new Response<Events>(true, new Error("Errore durante aggiornamento evento"));
                }
            }
            catch (Exception ex)
            {
                return new Response<Events>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<EventSubscription>> SubscribeEvent(EventSubscription subscriptioneEvent)
        {
            IscrizioneEvento iscrizione = new IscrizioneEvento();
            bool IsInserted = true;

            try
            {
                iscrizione = Mapper.Map<EventSubscription, IscrizioneEvento>(subscriptioneEvent);

                var CheckIscrizione = await dalEventi.CheckIscrizione(iscrizione).ConfigureAwait(false);

                if (CheckIscrizione is null)
                {
                    IsInserted &= await dalEventi.IscrizioneEvento(iscrizione).ConfigureAwait(false);
                }
                else if(CheckIscrizione is not null && CheckIscrizione.Cancellata.HasValue && CheckIscrizione.Cancellata.Value)
                {
                    IsInserted &= await dalEventi.UpdateIscrizioneEvento(iscrizione).ConfigureAwait(false);
                }
                else
                {
                    return new Response<EventSubscription>(false, new Error(ErrorCode.IscrizioneEventoGiaEffettuata, "E' gia stata effettuata un iscrizione per queste categorie"));
                }

                if (IsInserted)
                {
                    return new Response<EventSubscription>(true, subscriptioneEvent);
                }
                else
                {
                    return new Response<EventSubscription>(false, new Error("Errore durante iscrizione"));
                }


            }
            catch (Exception ex)
            {
                return new Response<EventSubscription>(false, new Error(ex.Message));
            }

        }

        public async Task<Response<List<SingleEventSubscription>>> GetEventSubscription(Guid UserId)
        {
            try
            {
                List<SingleEventSubscription> subscriptions = new();
                List<IscrizioneEvento> iscrizioni = new();

                iscrizioni = await dalEventi.GetIscrizioni(UserId).ConfigureAwait(false);

                subscriptions = Mapper.Map<List<IscrizioneEvento>, List<SingleEventSubscription>>(iscrizioni);

                return new Response<List<SingleEventSubscription>>(true, subscriptions);
            }
            catch (Exception ex)
            {
                return new Response<List<SingleEventSubscription>>(false, new Error(ex.Message));
            }

        }

        public async Task<Response<SingleEventSubscription>> DeleteEventSubscription(EventSubscription Subscription)
        {
            try
            {
                SingleEventSubscription subscription = new();
                IscrizioneEvento iscrizione = new();

                iscrizione = Mapper.Map<EventSubscription, IscrizioneEvento>(Subscription);

                bool result = await dalEventi.UpdateIscrizioneEvento(iscrizione).ConfigureAwait(false);

                if (result)
                {
                    return new Response<SingleEventSubscription>(true, subscription);
                }
                else
                {
                    return new Response<SingleEventSubscription>(false, new Error("Errore durante cancellazione iscrizione"));
                }
            }
            catch (Exception ex)
            {
                return new Response<SingleEventSubscription>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Competitor>>> GetCompetitors(Guid EventId)
        {
            List<Competitor> competitors = new();

            try
            {
                List<IscrizioneEvento> subscriptions = new();

                subscriptions = await dalEventi.GetIscrizioniByEvento(EventId).ConfigureAwait(false);

                foreach(IscrizioneEvento sub in subscriptions)
                {
                    Competitor competitor = new();
                    competitor.Iscrizione = Mapper.Map<IscrizioneEvento, SingleEventSubscription>(sub);

                    Utente? utente = await dalUtente.GetUtente(sub.IdUtente).ConfigureAwait(false);
                    if(utente is not null)
                    {
                        competitor.Utente = Mapper.Map<Utente, User>(utente);
                        //competitor.Utente.Nome = utente.Nome;
                        //competitor.Utente.Cognome = utente.Cognome;
                        //competitor.Utente.CodFiscale = utente.CodFiscale;
                        //competitor.Utente.Email = utente.Email;
                    }

                    Evento? evento = await dalEventi.GetEvent(sub.IdEvento).ConfigureAwait(false);
                    if(evento is not null)
                    {
                        competitor.Evento.NomeEvento = evento.NomeEvento;
                        competitor.Evento.Descrizione = evento.Descrizione;
                        competitor.Evento.DataInizioEvento = new DateTime(evento.DataInizioEvento.Year, evento.DataInizioEvento.Month, evento.DataInizioEvento.Day);
                        competitor.Evento.DataFineEvento = new DateTime(evento.DataFineEvento.Year, evento.DataFineEvento.Month, evento.DataFineEvento.Day);
                    }


                    competitors.Add(competitor);
                }

                return new Response<List<Competitor>>(true, competitors);
            }
            catch (Exception ex)
            {
                return new Response<List<Competitor>>(false, new Error(ex.Message));
            }

        }

        public async Task<Response<DocumentExt>> CreateCompetitorExcel(Guid EventId)
        {
            List<Competitor> competitors = new();

            try
            {
                byte[] fileContents;
                List<IscrizioneEvento> subscriptions = new();
                List<Categoria> categorie = new();
                DocumentExt excel = new();

                subscriptions = await dalEventi.GetIscrizioniByEvento(EventId).ConfigureAwait(false);

                foreach (IscrizioneEvento sub in subscriptions)
                {
                    Competitor competitor = new();
                    competitor.Iscrizione = Mapper.Map<IscrizioneEvento, SingleEventSubscription>(sub);

                    Utente? utente = await dalUtente.GetUtente(sub.IdUtente).ConfigureAwait(false);
                    if (utente is not null)
                    {
                        competitor.Utente = Mapper.Map<Utente, User>(utente);
                    }

                    Evento? evento = await dalEventi.GetEvent(sub.IdEvento).ConfigureAwait(false);
                    if (evento is not null)
                    {
                        competitor.Evento.NomeEvento = evento.NomeEvento;
                        competitor.Evento.Descrizione = evento.Descrizione;
                        competitor.Evento.DataInizioEvento = new DateTime(evento.DataInizioEvento.Year, evento.DataInizioEvento.Month, evento.DataInizioEvento.Day);
                        competitor.Evento.DataFineEvento = new DateTime(evento.DataFineEvento.Year, evento.DataFineEvento.Month, evento.DataFineEvento.Day);
                    }

                    competitors.Add(competitor);
                }

                categorie = await dalEventi.GetCategorie().ConfigureAwait(false);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var cat = competitors.Select(c => c.Iscrizione.Categoria).ToList();

                    foreach (var c in cat)
                    {
                        var worksheet = package.Workbook.Worksheets.Add(categorie.FirstOrDefault(cat => cat.Id.Equals(c)).Descrizione);

                        int riga = 1;

                        worksheet.Cells[riga, 1].Value = "Nome";
                        worksheet.Cells[riga, 2].Value = "Cognome";

                        riga++;

                        foreach (var iscritti in competitors.Where(comp => comp.Iscrizione.Categoria.Equals(c)))
                        {
                            worksheet.Cells[riga, 1].Value = iscritti.Utente.Nome;
                            worksheet.Cells[riga, 2].Value = iscritti.Utente.Cognome;

                            riga++;
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        fileContents = stream.ToArray();
                    }

                    excel.datiDocumento = Convert.ToBase64String(fileContents);
                    excel.nomeDocumento = string.Concat(competitors.FirstOrDefault().Evento.NomeEvento, '_', DateTime.Now.ToString("yyyyMMddhhmmss"));
                }
                //File.WriteAllBytes("C:\\Users\\luca.locci\\Desktop\\testexcel.xlsx", fileContents);
                return new Response<DocumentExt>(true, excel);
            }            
            catch(Exception ex)
            {
                return new Response<DocumentExt>(false, new Error(ex.Message));

            }
        }

        public async Task<Response<Competition>> AddCompetition(Competition competition)
        {
            Gare gara = new();
            bool IsCompetitionCreated = false;
            SequentialGuidValueGenerator generator = new();

            try
            {
                //evento = Mapper.Map<Events, Evento>(events);
                gara.Id = await generator.NextAsync(null).ConfigureAwait(false);
                gara.Evento = competition.Event;
                gara.Nome = competition.Name;
                gara.Categoria = competition.Category;
                gara.ImportoIscrizione = competition.CompetitionFee;

                IsCompetitionCreated = await dalEventi.AddGara(gara).ConfigureAwait(false);

                if (IsCompetitionCreated)
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetCompetitionsByEvent_), competition.Event.ToString()));

                    return new Response<Competition>(true, competition);
                }
                else
                {
                    throw new Exception("Errore creazione gara");
                }

            }
            catch (Exception ex)
            {
                return new Response<Competition>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Competition>>> GetCompetitionByEvent(Guid EventId)
        {
            try 
            { 
                List<Competition> competitions = new();
                List<Gare> gare = new();

                competitions = await HybridCache.GetOrCreateAsync<List<Competition>>(string.Concat(nameof(CacheKey.GetCompetitionsByEvent_),EventId.ToString()), 
                    async result => Mapper.Map<List<Competition>>(await dalEventi.GetGareByIdEvento(EventId).ConfigureAwait(false)));

                return new Response<List<Competition>>(true, competitions);
            }
            catch (Exception ex)
            {
                return new Response<List<Competition>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Competition>> DeleteCompetition(Guid CompetitionId, Guid EventId)
        {
            bool IsCompetitionDeleted = false;

            try
            {
                IsCompetitionDeleted = await dalEventi.DeleteGara(CompetitionId).ConfigureAwait(false);

                if (IsCompetitionDeleted)
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetCompetitionsByEvent_), EventId.ToString()));

                    return new Response<Competition>(true, new Competition());
                }
                else
                {
                    throw new Exception("Errore creazione gara");
                }

            }
            catch (Exception ex)
            {
                return new Response<Competition>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Competition>>> GetCompetitionsByEventAndUser(Guid EventId, Guid UserId)
        {
            try
            {
                List<Competition> competitions = new();
                List<Gare> gare = new();

                competitions = Mapper.Map<List<Competition>>(await dalEventi.GetGareByIdEvento(EventId).ConfigureAwait(false));

                foreach (var competition in competitions)
                {
                    var iscrizione = new IscrizioneEvento()
                    {
                        IdEvento = EventId,
                        IdUtente = UserId,
                        Gara = competition.Id.Value
                    };

                    var check = await dalEventi.CheckIscrizione(iscrizione).ConfigureAwait(false);

                    if(check != null)
                        competition.Subscribed = check.Cancellata.HasValue && !check.Cancellata.Value;

                    competition.UserId = UserId;
                }

                return new Response<List<Competition>>(true, competitions);
            }
            catch (Exception ex)
            {
                return new Response<List<Competition>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<DocumentExt>> GetCompetitionSubscriptionReportByUser(Guid EventId, Guid UserId)
        {
            try
            {
                TemplateDTO template = new();
                List<IscrizioneEvento> iscrizioni = new();
                DocumentExt output = new();
                Events events = new();

                template = await HybridCache.GetOrCreateAsync<TemplateDTO>(nameof(TemplateName.ReportIscrizioneUtente),
                    async result => Mapper.Map<TemplateDTO>(await dalSito.GetTemplateByName(nameof(TemplateName.ReportIscrizioneUtente)).ConfigureAwait(false)));

                if(template is null)
                {
                    throw new Exception("Errore generazione report, template mancante");
                }

                var evento = await HybridCache.GetOrCreateAsync<Evento>(string.Concat(nameof(CacheKey.GetEvent), '_', EventId),
                    async result => await dalEventi.GetEvent(EventId).ConfigureAwait(false));
                events.NomeEvento = evento.NomeEvento;
                events.ImportoIscrizione = evento.ImportoIscrizione;

                iscrizioni = await dalEventi.GetIscrizioniByEventoEUtente(EventId, UserId).ConfigureAwait(false);

                if(iscrizioni is null
                    || !iscrizioni.Any())
                {
                    throw new Exception("Non hai ancora effettuato nessuna iscrizione");
                }

                decimal totale = 0m;

                var sb = new StringBuilder();
                sb.AppendLine("<table>");
                sb.AppendLine("<tr><th>Nome</th><th>Note</th><th>Importo</th></tr>");

                foreach (var iscrizione in iscrizioni)
                {
                    var gara = await dalEventi.GetGareById(iscrizione.Gara).ConfigureAwait(false);

                    sb.AppendLine($"<tr><td>{gara.Nome}</td><td>{iscrizione.Note}</td><td>{gara.ImportoIscrizione:C}</td></tr>");

                    totale += gara.ImportoIscrizione.HasValue ? gara.ImportoIscrizione.Value : 0;
                }

                //caso costo iscrizione unico per evento e non per singola gara
                if(totale <= 0 
                    && events.ImportoIscrizione.HasValue && events.ImportoIscrizione != 0) 
                {
                    totale = events.ImportoIscrizione.Value;
                }

                sb.AppendLine("</table>");

                //replace lista iscrizioni su template
                template.TemplateBodyHtml = template.TemplateBodyHtml!.Replace("{{ListaIscrizioni}}", sb.ToString());
                //replace totale su template
                template.TemplateBodyHtml = template.TemplateBodyHtml!.Replace("{{ImportoTotaleIscrizioni}}", totale.ToString("C"));

                var pdf = new PDFGenerator(template.TemplateBodyHtml.ToString(), template.TemplateHeaderHtml, template.TemplateFooterHtml);

                byte[]? pdfBytes = pdf.GeneratePDF();

                if(pdfBytes != null)
                {
                    output.datiDocumento = Convert.ToBase64String(pdfBytes);
                    output.nomeDocumento = string.Concat("ReportIscrizioni_", evento.NomeEvento, ".pdf");
                    
                    return new Response<DocumentExt>(true, output);
                }
                else
                {
                    throw new Exception("Errore creazione report iscrizioni evento");
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentExt>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<DocumentExt>> GetCompetitionSubscriptionReportByMaster(Guid EventId, Guid Org)
        {
            try
            {
                TemplateDTO template = new();
                List<IscrizioneEvento> iscrizioni = new();
                DocumentExt output = new();
                Events events = new();

                template = await HybridCache.GetOrCreateAsync<TemplateDTO>(nameof(TemplateName.ReportIscrizioniDaMaestro),
                    async result => Mapper.Map<TemplateDTO>(await dalSito.GetTemplateByName(nameof(TemplateName.ReportIscrizioniDaMaestro)).ConfigureAwait(false)));

                if (template is null)
                {
                    throw new Exception("Errore generazione report, template mancante");
                }

                var evento = await HybridCache.GetOrCreateAsync<Evento>(string.Concat(nameof(CacheKey.GetEvent), '_', EventId),
                    async result => await dalEventi.GetEvent(EventId).ConfigureAwait(false));
                events.NomeEvento = evento.NomeEvento;
                events.ImportoIscrizione = evento.ImportoIscrizione;

                iscrizioni = await dalEventi.GetIscrizioniByEventoEOrg(EventId, Org).ConfigureAwait(false);

                if (iscrizioni is null
                    || !iscrizioni.Any())
                {
                    throw new Exception("Non hai ancora effettuato nessuna iscrizione");
                }

                decimal totale = 0m;

                var sb = new StringBuilder();
                sb.AppendLine("<table>");
                sb.AppendLine("<tr><th>Nome Atleta</th><th>Nome Gara</th><th>Note</th><th>Importo</th></tr>");

                foreach (var iscrizione in iscrizioni)
                {
                    var gara = await dalEventi.GetGareById(iscrizione.Gara).ConfigureAwait(false);
                    var utente = await dalUtente.GetUtente(iscrizione.IdUtente).ConfigureAwait(false);

                    if (utente is null)
                    {
                        throw new Exception("Errore recupero atleta");
                    }

                    sb.AppendLine($"<tr><td>{string.Concat(utente.Nome, ' ', utente.Cognome)}</td><td>{gara.Nome}</td><td>{iscrizione.Note}</td><td>{gara.ImportoIscrizione:C}</td></tr>");

                    totale += gara.ImportoIscrizione.HasValue ? gara.ImportoIscrizione.Value : 0;
                }

                //caso costo iscrizione unico per evento e non per singola gara
                if (totale <= 0
                    && events.ImportoIscrizione.HasValue && events.ImportoIscrizione != 0)
                {
                    totale = events.ImportoIscrizione.Value;
                }

                sb.AppendLine("</table>");

                //replace lista iscrizioni su template
                template.TemplateBodyHtml = template.TemplateBodyHtml!.Replace("{{ListaIscrizioni}}", sb.ToString());
                //replace totale su template
                template.TemplateBodyHtml = template.TemplateBodyHtml!.Replace("{{ImportoTotaleIscrizioni}}", totale.ToString("C"));

                var pdf = new PDFGenerator(template.TemplateBodyHtml!.ToString(), template.TemplateHeaderHtml, template.TemplateFooterHtml);

                byte[]? pdfBytes = pdf.GeneratePDF();

                if (pdfBytes != null)
                {
                    output.datiDocumento = Convert.ToBase64String(pdfBytes);
                    output.nomeDocumento = string.Concat("ReportIscrizioni_", evento.NomeEvento, ".pdf");

                    return new Response<DocumentExt>(true, output);
                }
                else
                {
                    throw new Exception("Errore creazione report iscrizioni evento");
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentExt>(false, new Error(ex.Message));
            }
        }

    }
}
