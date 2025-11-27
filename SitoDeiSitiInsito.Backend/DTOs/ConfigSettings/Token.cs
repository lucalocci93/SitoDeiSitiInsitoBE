namespace SitoDeiSiti.Models.ConfigSettings
{
    public class Token
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double expireMinutes { get; set; }
    }
}
