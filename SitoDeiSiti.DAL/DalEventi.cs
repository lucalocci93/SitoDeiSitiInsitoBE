using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;

namespace SitoDeiSiti.DAL
{
    public class DalEventi : DalBase, IDalEventi
    {
        public DalEventi(SitoDeiSitiInsitoContext context) :
            base(context)
        {
            
        }

        public async Task<IscrizioneEvento?> CheckIscrizione(IscrizioneEvento iscrizioneEvento)
        {
            try
            {
                IscrizioneEvento? iscrizione = await Db.IscrizioneEvento
                    .FirstOrDefaultAsync(i => i.IdEvento.Equals(iscrizioneEvento.IdEvento)
                            && i.IdUtente.Equals(iscrizioneEvento.IdUtente)
                            && i.Gara.Equals(iscrizioneEvento.Gara));

                return iscrizione;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateEvento(Evento evento)
        {
            try
            {
                Db.Evento.Add(evento);

                await Db.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateIscrizioneEvento(IscrizioneEvento IscrizioneEvento)
        {
            int UpdatedRow = 0;
            try
            {
                UpdatedRow = await Db.IscrizioneEvento.Where(u => u.IdEvento == IscrizioneEvento.IdEvento 
                                                                && u.IdUtente == IscrizioneEvento.IdUtente
                                                                && u.Gara == IscrizioneEvento.Gara)
                    .ExecuteUpdateAsync(setter =>
                        setter
                        .SetProperty(p => p.Cancellata, IscrizioneEvento.Cancellata)
                        .SetProperty(p => p.Note, IscrizioneEvento.Note)
                    ).ConfigureAwait(false);

                if (UpdatedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Categoria>> GetCategorie()
        {
            List<Categoria> categorie = new();

            try
            {
                categorie = await Db.Categoria.ToListAsync()
                                              .ConfigureAwait(false);

                return categorie;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Evento?> GetEvent(Guid Id)
        {
            try
            {
                Evento? evento = new();

                evento = await Db.Evento
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id.Equals(Id));

                return evento;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Evento>> GetEventi()
        {
            try
            {
                List<Evento> events = new();

                events = await Db.Evento
                        .AsNoTracking()
                        .OrderByDescending(d => d.DataInizioEvento)
                        .ToListAsync()
                        .ConfigureAwait(false);

                return events;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<List<Gare>> GetGareByIdEvento(Guid EventId)
        {
            try
            {
                List<Gare> gare = new();

                gare = await Db.Gare
                        .AsNoTracking()
                        .Where(g => g.Evento.Equals(EventId))
                        .ToListAsync()
                        .ConfigureAwait(false);

                return gare;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddGara(Gare gara)
        {
            int InsertedRows = 0;

            try
            {
                Db.Gare.Add(gara);

                InsertedRows = await Db.SaveChangesAsync().ConfigureAwait(false);

                if (InsertedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }   
        }

        public async Task<List<IscrizioneEvento>> GetIscrizioni(Guid UserId)
        {
            try
            {
                List<IscrizioneEvento> iscrizioni = new();

                iscrizioni = await Db.IscrizioneEvento
                    .AsNoTracking()
                    .Where(e => e.IdUtente.Equals(UserId))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return iscrizioni;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IscrizioneEvento>> GetIscrizioniByEvento(Guid EventId)
        {
            try
            {
                List<IscrizioneEvento> iscrizioni = new();

                iscrizioni = await Db.IscrizioneEvento
                    .AsNoTracking()
                    .Where(e => e.IdEvento.Equals(EventId))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return iscrizioni;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IscrizioneEvento(IscrizioneEvento IscrizioneEvento)
        {
            int InsertedRow = 0;
            try
            {
                Db.IscrizioneEvento.Add(IscrizioneEvento);

                InsertedRow = await Db.SaveChangesAsync();

                if (InsertedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateEvento(Evento evento)
        {
            int UpdatedRow = 0;
            try
            {
                UpdatedRow = await Db.Evento.Where(u => u.Id == evento.Id)
                    .ExecuteUpdateAsync(setter =>
                        setter
                        .SetProperty(p => p.NomeEvento, evento.NomeEvento)
                        .SetProperty(p => p.LuogoEvento, evento.LuogoEvento)
                        .SetProperty(p => p.Descrizione, evento.Descrizione)
                        .SetProperty(p => p.Link, evento.Link)
                        .SetProperty(p => p.DataInizioEvento, evento.DataInizioEvento)
                        .SetProperty(p => p.DataFineEvento, evento.DataFineEvento)
                    ).ConfigureAwait(false);

                if(UpdatedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteGara(Guid Id)
        {
            bool result = false;
            try
            {
                var gara = await Db.Gare.FindAsync(Id).ConfigureAwait(false);

                if (gara != null)
                {
                    Db.Gare.Remove(gara);
                    int rowdeleted = await Db.SaveChangesAsync().ConfigureAwait(false);

                    if (rowdeleted > 0 && rowdeleted < 2)
                    {
                        result = true;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<IscrizioneEvento>> GetIscrizioniByEventoEUtente(Guid EventId, Guid UserId)
        {
            try
            {
                var gara = await Db.IscrizioneEvento.Where(i =>
                        i.IdEvento.Equals(EventId) &&
                        i.IdUtente.Equals(UserId) &&
                        i.Cancellata.HasValue && !i.Cancellata.Value)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return gara;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Gare> GetGareById(Guid IdGara)
        {
            try
            {
                var gara = await Db.Gare.FindAsync(IdGara).ConfigureAwait(false);

                return gara;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IscrizioneEvento>> GetIscrizioniByEventoEOrg(Guid EventId, Guid Org)
        {
            try
            {
                var gara = await Db.IscrizioneEvento
                    .Where(i =>
                        i.IdEvento.Equals(EventId) &&
                        i.Cancellata.HasValue && !i.Cancellata.Value &&
                        i.IdUtenteNavigation.UtenteAtleta.Organizzazione.Equals(Org))
                    .OrderBy(i => i.IdUtente)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return gara;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
