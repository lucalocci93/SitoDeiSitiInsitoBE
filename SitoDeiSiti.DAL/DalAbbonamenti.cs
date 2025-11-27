using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL
{
    public class DalAbbonamenti : DalBase, IDalAbbonamenti
    {
        public DalAbbonamenti(SitoDeiSitiInsitoContext context)
            :base(context)
        {
            
        }

        public async Task<int> AddAbbonamenti(Abbonamento abbonamento)
        {
            int AddRow = 0;

            try
            {
                Db.Abbonamento.Add(abbonamento);

                AddRow = await Db.SaveChangesAsync().ConfigureAwait(false);

                return AddRow;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddTipoAbbonamento(TipoAbbonamento tipoAbbonamento)
        {
            int RowInserted = 0;
            try
            {
                Db.TipoAbbonamento.Add(tipoAbbonamento);

                RowInserted = await Db.SaveChangesAsync().ConfigureAwait(false);

                return RowInserted;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Abbonamento>> GetAbbonamentiUtente(Guid Utente)
        {
            List<Abbonamento> Abbonamenti = new();
            try
            {
                Abbonamenti = await Db.Abbonamento
                                    .AsNoTracking()
                                    .Include(a => a.TipoAbbonamentoNavigation)
                                    .AsSplitQuery()
                                    .Where(a => a.Utente == Utente)
                                    .OrderByDescending(a => a.DataScadenza)
                                    .ToListAsync()
                                    .ConfigureAwait(false);
                return Abbonamenti;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<Abbonamento?> GetAbbonamento(Guid Utente, int Id)
        {
            Abbonamento? abbonamento = new();
            try
            {
                abbonamento = await Db.Abbonamento
                                .AsNoTracking()
                                .FirstOrDefaultAsync(a => a.Id == Id && a.Utente == Utente);

                return abbonamento;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Abbonamento>> GetAllAbbonamenti()
        {
            List<Abbonamento> abbonamenti = new();

            try
            {
                abbonamenti = await Db.Abbonamento
                                     .AsNoTracking()
                                     .Include(a => a.TipoAbbonamentoNavigation)
                                     .AsSplitQuery()
                                     .ToListAsync()
                                     .ConfigureAwait(false);
                return abbonamenti;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyCollection<TipoAbbonamento>> GetTipiAbbonamento()
        {
            IReadOnlyCollection<TipoAbbonamento> tipiAbbonamento = new List<TipoAbbonamento>();
            try
            {
                tipiAbbonamento = await Db.TipoAbbonamento.AsNoTracking().ToListAsync().ConfigureAwait(false);

                return tipiAbbonamento;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdateAbbonamento(DbOperationsAbbonamentoEnums operation, Abbonamento abbonamento, TipoAbbonamento tipoAbbonamento)
        {
            int RowUpdated = 0;
            try
            {
                switch (operation)
                {
                    case DbOperationsAbbonamentoEnums.SospendiAbbonamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteUpdateAsync(setter =>
                                setter
                                    .SetProperty(a => a.DataScadenza, DateTime.Now)
                                    .SetProperty(a => a.Attivo, abbonamento.Pagato.HasValue && abbonamento.Pagato.Value && abbonamento.DataIscrizione >= DateTime.Now && abbonamento.DataScadenza <= DateTime.Now)
                            );

                            break;
                        }

                    case DbOperationsAbbonamentoEnums.EstendiAbbonamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteUpdateAsync(setter =>
                                setter
                                    .SetProperty(a => a.DataScadenza, abbonamento.DataScadenza)
                                    .SetProperty(a => a.Attivo, abbonamento.Pagato.HasValue && abbonamento.Pagato.Value && abbonamento.DataIscrizione >= DateTime.Now && abbonamento.DataScadenza <= DateTime.Now)
                            //.SetProperty(a => a.TipoAbbonamento, subscription.IdTipoAbbonamento)
                            );

                            break;
                        }

                    case DbOperationsAbbonamentoEnums.CambiaTipoAbbonamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteUpdateAsync(setter =>
                                setter
                                    .SetProperty(a => a.DataScadenza, abbonamento.DataScadenza)
                                    .SetProperty(a => a.TipoAbbonamento, tipoAbbonamento.Id)
                                    .SetProperty(a => a.Attivo, abbonamento.Pagato.HasValue && abbonamento.Pagato.Value && abbonamento.DataIscrizione >= DateTime.Now && abbonamento.DataScadenza <= DateTime.Now)
                            );

                            break;
                        }

                    case DbOperationsAbbonamentoEnums.AggiornaInfoPagamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteUpdateAsync(setter =>
                                setter
                                    .SetProperty(a => a.UrlPagamento, abbonamento.UrlPagamento)
                                    .SetProperty(a => a.IdCheckout, abbonamento.IdCheckout)
                            );

                            break;
                        }

                    //case DbOperationsAbbonamentoEnums.SetAbbonamentoPagato:
                    //    {
                    //        RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                    //            .ExecuteUpdateAsync(setter =>
                    //            setter
                    //                .SetProperty(a => a.Pagato, true)
                    //                .SetProperty(a => a.Attivo, abbonamento.DataIscrizione >= DateTime.Now && abbonamento.DataScadenza <= DateTime.Now)
                    //        );
                    //
                    //        break;
                    //    }

                    case DbOperationsAbbonamentoEnums.CancellaAbbonamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteDeleteAsync();
                            break;
                        }

                    case DbOperationsAbbonamentoEnums.AggiornaStatoAbbonamento:
                        {
                            RowUpdated = await Db.Abbonamento.Where(a => a.Utente == abbonamento.Utente && a.Id == abbonamento.Id)
                                .ExecuteUpdateAsync(setter =>
                                setter
                                    .SetProperty(a => a.Pagato, abbonamento.Pagato)
                                    .SetProperty(a => a.Attivo, abbonamento.Pagato.HasValue && abbonamento.Pagato.Value && abbonamento.DataIscrizione >= DateTime.Now && abbonamento.DataScadenza <= DateTime.Now)
                            );
                            break;
                        }
                }

                return RowUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
