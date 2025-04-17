
namespace SitoDeiSiti.DTOs
{
    public record User
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string CodFiscale { get; set; }
        public string Password { get; set; }
        public bool? IsAdmin { get; set; }
        public Guid? RowGuid { get; set; }

        //Info
        public DateOnly? DataNascita { get; set; }
        public string Via { get; set; }
        public string Numero { get; set; }
        public string Citta { get; set; }
        public string Regione { get; set; }
        public string Nazione { get; set; }

        //privacy
        public bool? ConsensoInvioMail { get; set; }

        //abbonamento
        public List<Subscription>? Abbonamenti { get; set; }
    }
}
