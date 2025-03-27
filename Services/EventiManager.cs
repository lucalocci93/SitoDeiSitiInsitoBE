using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Interfaces;
using SitoDeiSiti.Models;
using SitoDeiSiti.Models.ConfigSettings;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml.Linq;

namespace Identity.Services
{
    public class EventiManager : BaseManager, IEvents
    {
        private readonly IDalEventi dalEventi;
        private readonly IDalUtente dalUtente;
        public EventiManager(SitoDeiSitiInsitoContext context, IMapper mapper, CacheManager cacheManager)
            : base(mapper, cacheManager)
        {
            dalEventi = new DalEventi(context);
            dalUtente = new DalUtenti(context);
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

                IsEventCreated = await dalEventi.CreateEvento(evento).ConfigureAwait(false);

                if (IsEventCreated)
                {
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
            string Key = "EventCategories";

            List<Category> cacheResult = await CacheManager.GetAsync<List<Category>>(Key);
            if (cacheResult is null || !cacheResult.Any())
            {
                try
                {
                    categorie = await dalEventi.GetCategorie().ConfigureAwait(false);

                    categories = Mapper.Map<List<Categoria>, List<Category>>(categorie);

                    await CacheManager.SetAsync<List<Category>>(Key, categories);

                    return new Response<List<Category>>(true, categories);
                }
                catch (Exception ex)
                {
                    return new Response<List<Category>>(false, new Error(ex.Message));
                }
            }
            else
            {
                categories = cacheResult.ToList();
                return new Response<List<Category>>(true, categories);
            }
        }

        public async Task<Response<Events>> GetEvent(Guid Id)
        {
            Evento? evento = new Evento();
            Events e = new Events();
            Dictionary<Guid, Category> categories = new Dictionary<Guid, Category>();

            try
            {
                evento = await dalEventi.GetEvent(Id).ConfigureAwait(false);

                if (evento is not null)
                {
                    e.Id = evento.Id;
                    e.NomeEvento = evento.NomeEvento;
                    e.LuogoEvento = evento.LuogoEvento;
                    e.Descrizione = evento.Descrizione;
                    e.Link = evento.Link;

                    e.DataInizioEvento = new DateTime(evento.DataInizioEvento.Year, evento.DataInizioEvento.Month, evento.DataInizioEvento.Day);
                    e.DataFineEvento = new DateTime(evento.DataFineEvento.Year, evento.DataFineEvento.Month, evento.DataFineEvento.Day);

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
                eventi = await dalEventi.GetEventi().ConfigureAwait(false);

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
                if (evento is not null)
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
            bool IsAlreadySubscribed = false;
            try
            {
                foreach (int Id in subscriptioneEvent.Categories)
                {
                    iscrizione.IdEvento = subscriptioneEvent.EventId;
                    iscrizione.IdUtente = subscriptioneEvent.UserId;
                    iscrizione.Note = subscriptioneEvent.Note;
                    iscrizione.Categoria = Id;

                    IsAlreadySubscribed = await dalEventi.CheckIscrizione(iscrizione).ConfigureAwait(false);

                    if (!IsAlreadySubscribed)
                    {
                        IsInserted &= await dalEventi.IscrizioneEvento(iscrizione).ConfigureAwait(false);
                    }
                    else
                    {
                        return new Response<EventSubscription>(false, new Error(ErrorCode.IscrizioneEventoGiaEffettuata, "E' gia stata effettuata un iscrizione per queste categorie"));
                    }
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

        public async Task<Response<SingleEventSubscription>> DeleteEventSubscription(Guid EventId, Guid UserId, int Category)
        {
            try
            {
                SingleEventSubscription subscription = new();
                IscrizioneEvento iscrizione = new();

                iscrizione.IdEvento = EventId;
                iscrizione.IdUtente = UserId;
                iscrizione.Categoria = Category;

                bool result = await dalEventi.DeleteIscrizione(iscrizione).ConfigureAwait(false);

                subscription = Mapper.Map<IscrizioneEvento, SingleEventSubscription>(iscrizione);

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
    }
}
