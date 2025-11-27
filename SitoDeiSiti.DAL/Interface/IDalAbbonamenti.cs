using DAL.Enums;
using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL.Interface
{
    public interface IDalAbbonamenti
    {
        public Task<int> AddAbbonamenti(Abbonamento abbonamento);
        public Task<List<Abbonamento>> GetAbbonamentiUtente(Guid Utente);
        public Task<Abbonamento?> GetAbbonamento (Guid Utente, int Id);
        public Task<List<Abbonamento>> GetAllAbbonamenti();
        public Task<int> UpdateAbbonamento(DbOperationsAbbonamentoEnums operation, Abbonamento abbonamento, TipoAbbonamento tipoAbbonamento);


        public Task<int> AddTipoAbbonamento(TipoAbbonamento tipoAbbonamento);
        public Task<IReadOnlyCollection<TipoAbbonamento>> GetTipiAbbonamento();
    }
}
