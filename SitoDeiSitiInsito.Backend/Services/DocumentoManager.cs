using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.Models.ConfigSettings;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using Microsoft.Extensions.Caching.Hybrid;
using SitoDeiSiti.Backend.Interfaces;

namespace SitoDeiSiti.Backend.Services
{
    public class DocumentoManager : BaseManager, IDocument
    {
        private readonly IDalDocumenti dalDocumenti;
        public DocumentoManager(SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache)
            : base(mapper, hybridCache)
        {
            dalDocumenti = new DalDocumenti(context);
        }

        private enum CacheKey
        {
            DocumentType,
            GetUserDocument,
            GetDocument
        }

        public async Task<Response<Document>> AddDocument(DocumentExt document)
        {
            #region file
            //try
            //{
            //
            //    //if (document.File == null || document.File.Length == 0)
            //    //    return new Response<Document>(false, new Error("File non selezionato"));
            //
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        //await document.File.CopyToAsync(memoryStream);
            //        var fileBytes = memoryStream.ToArray();
            //        document.DatiDocumento = Convert.FromBase64String(fileBytes);
            //    }
            //}           
            //catch(Exception ex)
            //{
            //
            //}
            #endregion

            //Documento doc = Mapper.Map<Document, Documento>(document);

            int addRows = 0;

            try
            {
                SequentialGuidValueGenerator sequentialGuidValueGenerator = new SequentialGuidValueGenerator();

                Documento doc = new Documento()
                {
                    UtenteId = document.rowGuid,
                    TipoDocumentoId = document.idTipoDocumento,
                    NomeDocumento = document.nomeDocumento,
                    DataCaricamento = document.dataCaricamento.HasValue ? document.dataCaricamento.Value : DateTime.Today,
                    DatiDocumento = Convert.FromBase64String(document.datiDocumento)
                };

                addRows = await dalDocumenti.AddDocumento(doc).ConfigureAwait(false);

                if(addRows == 1)
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetUserDocument), '_', document.rowGuid)).ConfigureAwait(false);

                    return new Response<Document>(true, document);
                }
                else
                {
                    return new Response<Document>(false, document);
                }
            }
            catch (Exception ex)
            {
                return new Response<Document>(false, new Error(ex.Message));
            }

        }

        public async Task<Response<List<Document>>> GetAllDocumentByUser(Guid User)
        {
            List<Documento> documents = new List<Documento>();
            List<Document> ListUserDocuments = new List<Document>();

            try
            {
                //documents = await dalDocumenti.GetAllDocumenti(User).ConfigureAwait(false);
                //ListUserDocuments = Mapper.Map<List<Documento>, List<Document>>(documents);

                ListUserDocuments = await HybridCache.GetOrCreateAsync(string.Concat(nameof(CacheKey.GetUserDocument), '_', User), async result => Mapper.Map<List<Document>>(await dalDocumenti.GetAllDocumenti(User).ConfigureAwait(false)));

                return new Response<List<Document>>(true, ListUserDocuments);
            }
            catch (Exception ex)
            {
                return new Response<List<Document>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<DocumentExt>> GetDocumentById(Guid Id)
        {
            Documento? documento = new Documento();
            DocumentExt Documento = new DocumentExt();

            try
            {
                //documento = await dalDocumenti.GetDocumento(Id).ConfigureAwait(false);
                Documento = await HybridCache.GetOrCreateAsync(string.Concat(nameof(CacheKey.GetDocument), '_', Id), async result => Mapper.Map<DocumentExt>(await dalDocumenti.GetDocumento(Id).ConfigureAwait(false)));

                if (documento != null)
                {
                    //Documento = Mapper.Map<Documento, DocumentExt>(documento);

                    return new Response<DocumentExt>(true, Documento);
                }
                else
                {
                    return new Response<DocumentExt>(false, new DocumentExt());

                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentExt>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<DocumentType>>> GetDocumentType()
        {
            List<DocumentType> documentTypes = new List<DocumentType>();
            List<TipoDocumento> tipoDocumenti = new List<TipoDocumento>();

            try
            {
                documentTypes = await HybridCache.GetOrCreateAsync(nameof(CacheKey.DocumentType), async result => Mapper.Map<List<DocumentType>>(await dalDocumenti.GetTipiAbbonamento().ConfigureAwait(false)));

                if(documentTypes != null && documentTypes.Any())
                {
                    return new Response<List<DocumentType>>(true, documentTypes);
                }
                else
                {
                    return new Response<List<DocumentType>>(false, new Error("Nessun tipo documento trovato"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentType>>(false, new Error(ex.Message));
            }
        }
    }
}
