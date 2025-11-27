using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL.Interface
{
    public interface IDalDocumenti
    {
        public Task<int> AddDocumento(Documento documento);
        public Task<List<Documento>> GetAllDocumenti(Guid Utente);
        public Task<Documento?> GetDocumento(Guid Id);


        public Task<List<TipoDocumento>> GetTipiAbbonamento();
    }
}
