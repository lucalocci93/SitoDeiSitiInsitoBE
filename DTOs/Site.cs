namespace SitoDeiSiti.DTOs
{
    public record Images
    {
        public int Id { get; set; }

        public string UrlImage { get; set; }

        public int? Page { get; set; }

        public int? Section { get; set; }

        public bool? UrlFromGoogleDrive { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
        public string AdditionalText { get; set; }
        
        public int? Order { get; set; }

    }

    public record Pages
    {
        public int Id { get; set; }
        public string Page { get; set; }
    }
}
