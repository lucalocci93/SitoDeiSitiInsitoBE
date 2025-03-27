using AutoMapper;
using Microsoft.Extensions.Options;
using Identity.Interfaces;
using Microsoft.EntityFrameworkCore;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.Models.ConfigSettings;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace Identity.Services
{
    public class DocumentoManager : BaseManager, IDocument
    {
        private readonly IDalDocumenti dalDocumenti;
        public DocumentoManager(SitoDeiSitiInsitoContext context, IMapper mapper, CacheManager cacheManager)
            : base(mapper, cacheManager)
        {
            dalDocumenti = new DalDocumenti(context);
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
                documents = await dalDocumenti.GetAllDocumenti(User).ConfigureAwait(false);

                ListUserDocuments = Mapper.Map<List<Documento>, List<Document>>(documents);

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
                documento = await dalDocumenti.GetDocumento(Id).ConfigureAwait(false);

                if(documento != null)
                {
                    Documento = Mapper.Map<Documento, DocumentExt>(documento);

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

            string key = "DocumentType";
            List<DocumentType> cacheResult = await CacheManager.GetAsync<List<DocumentType>>(key);
            if (cacheResult is null)
            {
                try
                {
                    tipoDocumenti = await dalDocumenti.GetTipiAbbonamento().ConfigureAwait(false);

                    documentTypes = Mapper.Map<List<TipoDocumento>, List<DocumentType>>(tipoDocumenti);
                    await CacheManager.SetAsync(key, documentTypes);

                    return new Response<List<DocumentType>>(true, documentTypes);
                }
                catch (Exception ex)
                {
                    return new Response<List<DocumentType>>(false, new Error(ex.Message));

                }

            }
            else
            {
                documentTypes = cacheResult.ToList();
                return new Response<List<DocumentType>>(true, documentTypes);
            }
        }
    }
}
