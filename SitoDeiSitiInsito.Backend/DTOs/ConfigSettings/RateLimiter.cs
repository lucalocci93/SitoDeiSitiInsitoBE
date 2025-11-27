namespace SitoDeiSiti.DTOs.ConfigSettings
{
    public class RateLimiter
    {
        public bool EnableRateLimiting {get;set;}
        public int PermitLimit        {get;set;}
        public int IntervalInSeconds  {get;set;}
        public int QueueLimit { get; set; }

    }
}
