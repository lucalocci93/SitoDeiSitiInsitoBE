namespace SitoDeiSiti.DTOs
{
    public record Subscription
    {
        public int? Id { get; set; }
        public int IdTipoAbbonamento { get; set; }
        public string TipoAbbonamento { get; set; }
        public DateTime DataIscrizione { get; set; }
        public DateTime? DataScadenza { get; set; }
        public string UrlPagamento { get; set; }
        public decimal? Importo { get; set; }
        public string IdCheckout { get; set; }
        public bool IsActive { get; set; }
        public bool? IsPayed { get; set; }
        public Guid Utente { get; set; }
    }

    public record SubscriptionType
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public int? GiorniDurata { get; set; }
        public bool ScadMensile { get; set; }
        public bool ScadSettimanale { get; set; }
        public bool ScadGiornaliera { get; set; }
    }
}
