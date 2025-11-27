using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL
{
    public class DalDocumenti : DalBase, IDalDocumenti
    {
        public DalDocumenti(SitoDeiSitiInsitoContext context)
            :base(context)
        {
            
        }

        public async Task<int> AddDocumento(Documento documento)
        {
            SequentialGuidValueGenerator generator = new SequentialGuidValueGenerator();
            int addRows = 0;

            try
            {
                documento.IdDocumento = await generator.NextAsync(null).ConfigureAwait(false);

                Db.Documento.Add(documento);
                addRows = await Db.SaveChangesAsync();

                return addRows;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Documento>> GetAllDocumenti(Guid Utente)
        {
            List<Documento> list = new();

            try
            {
                list = await Db.Documento
                               .AsNoTracking()
                               //.Include(a => a.TipoDocumento)
                               //.AsSplitQuery()
                               .Where(a => a.UtenteId == Utente)
                               .OrderByDescending(a => a.DataCaricamento)
                               .ToListAsync()
                               .ConfigureAwait(false);

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Documento?> GetDocumento(Guid Id)
        {
            Documento? documento = new();

            try
            {
                documento = await Db.Documento
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(a => a.IdDocumento == Id)
                                    .ConfigureAwait(false);
                return documento;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TipoDocumento>> GetTipiAbbonamento()
        {
            List<TipoDocumento> list = new();

            try
            {
                list = await Db.TipoDocumento
                               .AsNoTracking()
                               .ToListAsync()
                               .ConfigureAwait(false);

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
