namespace SitoDeiSiti.DTOs
{
    public record SumUp
    {
        public string ClientId      {get;set;}
        public string ClientSecret  {get;set;}
        public string MerchantCode { get; set; }
        public string GrantType { get; set; }
        public string SumUpAuthUrl { get; set; }
        public string SumUpCheckoutUrl { get; set; }
    }
}
