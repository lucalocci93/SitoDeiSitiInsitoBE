using Identity.DTOs;
using Identity.Models;

namespace Identity.Interfaces
{
    public interface IDocument
    {
        Task<Response<List<Document>>> GetAllDocumentByUser(Guid User);

        Task<Response<DocumentExt>> GetDocumentById(Guid Id);

        Task<Response<Document>> AddDocument(DocumentExt document);

        Task<Response<List<DocumentType>>> GetDocumentType();
    }
}
