using System.Text.Json.Serialization;

namespace SitoDeiSiti.DTOs
{
    public record Graphics
    {
        public int Id { get; set; }

        public string UrlImage { get; set; }

        public int? Page { get; set; }

        public int? Section { get; set; }

        public bool? UrlFromGoogleDrive { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AdditionalText { get; set; }

        public bool? IsAdditionalTextMarkdown { get; set; }

        public int? Order { get; set; }

        public bool Active { get; set; }

    }

    public record Pages
    {
        public int Id { get; set; }
        public string Page { get; set; }
    }

    public record Redirections
    {
        public int? id { get; set; }
        public string url { get; set; }
        public string redirectUrl { get; set; }
        public bool active { get; set; }
    }

    public record Videos
    {
        public int? Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Provider { get; set; }
        public bool Active { get; set; }
    }

    public record Notification
    {
        public Guid? Id { get; set; }
        public int Page { get; set; }
        public string? Text { get; set; }
        public bool Active { get; set; }
    }

    public record NotificationEvent
    {
        public bool SendEvent { get; set; } = false;
        public List<Notification>? Notifications { get; set; }
    }
}
