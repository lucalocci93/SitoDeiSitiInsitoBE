namespace SitoDeiSiti.DTOs
{
    public record Document
    {
        public Guid? idDocumento { get; set; }
        public int idTipoDocumento { get; set; }
        public string nomeDocumento { get; set; }
        public DateTime? dataCaricamento { get; set; }
    }

    public record DocumentExt : Document
    {
        public Guid rowGuid { get; set; }
        public string datiDocumento { get; set; }
    }

    public record DocumentType
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
    }
}
