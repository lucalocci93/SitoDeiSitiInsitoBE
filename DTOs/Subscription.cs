namespace Identity.DTOs
{
    public class Subscription
    {
        public int? Id { get; set; }
        public int IdTipoAbbonamento { get; set; }
        public string TipoAbbonamento { get; set; }
        public DateOnly DataIscrizione { get; set; }
        public DateOnly? DataScadenza { get; set; }
        public bool IsActive { get; set; }
        public Guid Utente { get; set; }
    }

    public class SubscriptionType
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public int? GiorniDurata { get; set; }
        public bool ScadMensile { get; set; }
        public bool ScadSettimanale { get; set; }
        public bool ScadGiornaliera { get; set; }
    }
}
