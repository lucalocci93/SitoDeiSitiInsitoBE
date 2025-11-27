namespace SitoDeiSiti.DTOs
{
    public record JWT
    {
        public string Key { get; set; }
        public string Token { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime? Expiration {  get; set; }
    }
}
