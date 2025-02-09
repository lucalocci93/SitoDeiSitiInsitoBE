using Identity.Interfaces;
namespace SitoDeiSiti.DTOs
{
    public class Document
    {
        public Guid? idDocumento { get; set; }
        public int idTipoDocumento { get; set; }
        public string nomeDocumento { get; set; }
        public DateTime? dataCaricamento { get; set; }
    }

    public class DocumentExt : Document
    {
        public Guid rowGuid { get; set; }
        public string datiDocumento { get; set; }
    }

    public class DocumentType
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
    }
}
