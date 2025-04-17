using System.Text.Json.Serialization;

namespace SitoDeiSiti.DTOs
{
    public record Events
    {
        public Guid? Id { get; set; }
        public Copertina Copertina { get; set; }
        public string NomeEvento { get; set; }
        public DateTime DataInizioEvento { get; set; }
        public DateTime DataFineEvento { get; set; }
        public string LuogoEvento { get; set; }
        public List<Category> Categorie { get; set; }
        public string Descrizione { get; set; }
        public string Link { get; set; }

        public Events()
        {
            Categorie = new List<Category>();
        }
    }

    public record Category
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Descrizione")]
        public string Descrizione { get; set; }

        public Category(int Id, string Descrizione)
        {
            this.Id = Id;
            this.Descrizione = Descrizione;
        }
    }

    public record Copertina
    {
        public string ImageData { get; set; }
        public string ContentType { get; set; }

        public Copertina()
        {
            ImageData = string.Empty;
            ContentType = "image/jpeg";
        }

    }

    public record EventSubscription
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public List<int> Categories { get; set; }
        public string Note { get; set; }

        public EventSubscription()
        {
            Categories = new List<int>();
        }
    }

    public record SingleEventSubscription
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public int Categoria { get; set; }
        public string Note { get; set; }
    }

    public record Competitor
    {
        public User Utente { get; set; }
        public SingleEventSubscription Iscrizione { get; set; }
        public Events Evento { get; set; }

        public Competitor()
        {
            Utente = new();
            Iscrizione = new();
            Evento = new();
        }
    }
}
