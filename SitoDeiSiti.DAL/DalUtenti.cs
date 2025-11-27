using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;

namespace SitoDeiSiti.DAL
{
    public class DalUtenti : DalBase, IDalUtente
    {
        public DalUtenti(SitoDeiSitiInsitoContext sitoDeiSitiInsitoContext):
            base(sitoDeiSitiInsitoContext)
        {
            
        }

        public async Task<bool> GetUtenteByMail(string mail)
        {
            bool IsPresent = false;
            try
            {
                IsPresent = await Db.Utente.AsNoTracking().AnyAsync(u => u.Email == mail).ConfigureAwait(false);

                return IsPresent;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Utente?> CheckUtenteUserAndPassword(string UserName, string Password)
        {
            Utente? utente = new();

            try
            {
                utente = await Db.Utente
                    .AsNoTracking()
                    .Include(u => u.UtenteAtleta)
                    .FirstOrDefaultAsync(u => u.Email == UserName
                                    && u.Password == Password)
                    .ConfigureAwait(false);

                return utente;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateUtente(Utente utente, UtenteInfo utenteInfo, UtentePrivacy utentePrivacy, UtenteAtleta utenteAtleta)
        {
            SequentialGuidValueGenerator generator = new SequentialGuidValueGenerator();

            try
            {
                utente.RowGuid = await generator.NextAsync(null).ConfigureAwait(false);
                utenteInfo.RowGuid = utentePrivacy.RowGuid = utenteAtleta.Rowguid = utente.RowGuid;

                Db.Utente.Add(utente);
                Db.UtenteInfo.Add(utenteInfo);
                Db.UtentePrivacy.Add(utentePrivacy);
                Db.UtenteAtleta.Add(utenteAtleta);

                await Db.SaveChangesAsync().ConfigureAwait(false);

                return true;

            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public async Task<Utente?> GetUtente(Guid Rowguid)
        {
            Utente? utente = new();
            try
            {
                utente = await Db.Utente
                                .AsNoTracking()
                                .Include(a => a.UtenteInfo)
                                .Include(a => a.UtenteAtleta)
                                .Include(a => a.Abbonamento)
                                .ThenInclude(t => t.TipoAbbonamentoNavigation)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(u => u.RowGuid == Rowguid)
                                .ConfigureAwait(false);
                return utente;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Utente?>> GetUtenti()
        {
            List<Utente> utenti = new();
            try
            {
                utenti = await Db.Utente.AsNoTracking().ToListAsync().ConfigureAwait(false);

                return utenti;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdateUser(UserDbOperationEnum operation, Utente utente, UtenteInfo utenteInfo, UtentePrivacy utentePrivacy, UtenteAtleta utenteAtleta)
        {
            try
            {
                int UpdatedRow = 0;

                switch (operation)
                {
                    case UserDbOperationEnum.AggiornaInfo:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                    setter
                                        .SetProperty(p => p.Nome, utente.Nome)
                                        .SetProperty(p => p.Cognome, utente.Cognome)
                                        .SetProperty(p => p.CodFiscale, utente.CodFiscale)
                                        ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.SetAdmin:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                   .ExecuteUpdateAsync(setter =>
                                       setter
                                           .SetProperty(p => p.IsAdmin, utente.IsAdmin)
                                           ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.SetMaestro:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                   .ExecuteUpdateAsync(setter =>
                                       setter
                                           .SetProperty(p => p.IsMaestro, utente.IsMaestro)
                                           ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.AggiornaIndirizzo:
                        {
                            UpdatedRow = await Db.UtenteInfo.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                            .SetProperty(p => p.Numero, utenteInfo.Numero)
                                            .SetProperty(p => p.Via, utenteInfo.Via)
                                            .SetProperty(p => p.Citta, utenteInfo.Citta)
                                            .SetProperty(p => p.Regione, utenteInfo.Regione)
                                            .SetProperty(p => p.Nazione, utenteInfo.Nazione)
                                            ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.AggiornaConsensi:
                        {
                            UpdatedRow = await Db.UtentePrivacy.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                            .SetProperty(p => p.ConsensoInvioMail, utentePrivacy.ConsensoInvioMail)
                                            ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.AggiornaUser:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                            .SetProperty(p => p.Email, utente.Email)
                                            ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.AggiornaPassword:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                            .SetProperty(p => p.Password, utente.Password)
                                            ).ConfigureAwait(false);
                            break;
                        }

                    case UserDbOperationEnum.AggiornaAll:
                        {
                            UpdatedRow = await Db.Utente.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                        .SetProperty(p => p.Nome, utente.Nome)
                                        .SetProperty(p => p.Cognome, utente.Cognome)
                                        .SetProperty(p => p.CodFiscale, utente.CodFiscale)
                                        .SetProperty(p => p.Email, utente.Email)
                                        ).ConfigureAwait(false);

                            UpdatedRow += await Db.UtenteInfo.Where(u => u.RowGuid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                        .SetProperty(p => p.Numero, utenteInfo.Numero)
                                        .SetProperty(p => p.Via, utenteInfo.Via)
                                        .SetProperty(p => p.Citta, utenteInfo.Citta)
                                        .SetProperty(p => p.Regione, utenteInfo.Regione)
                                        .SetProperty(p => p.Nazione, utenteInfo.Nazione)
                                        ).ConfigureAwait(false);

                            UpdatedRow += await Db.UtenteAtleta.Where(u => u.Rowguid == utente.RowGuid)
                                .ExecuteUpdateAsync(setter =>
                                        setter
                                        .SetProperty(p => p.Organizzazione, utenteAtleta.Organizzazione)
                                        .SetProperty(p => p.Cintura, utenteAtleta.Cintura)
                                        ).ConfigureAwait(false);

                            break;
                        }
                }

                await Db.SaveChangesAsync().ConfigureAwait(false);

                return UpdatedRow;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Cinture>> GetCinture()
        {
            List<Cinture> cinture = new();
            try
            {
                cinture = await Db.Cinture.AsNoTracking().ToListAsync().ConfigureAwait(false);

                return cinture;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Organizzazioni>> GetOrganizzazioni()
        {
            List<Organizzazioni> organizzazioni = new();
            try
            {
                organizzazioni = await Db.Organizzazioni.AsNoTracking().ToListAsync().ConfigureAwait(false);

                return organizzazioni;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Utente>> GetUtentiOrganizzazioni(Guid Org)
        {
            List<Utente> utenti = new();
            try
            {
                utenti = await Db.Utente
                    .Include(u => u.UtenteAtleta)
                    .Where(u => u.UtenteAtleta.Organizzazione.Equals(Org))
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return utenti;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
