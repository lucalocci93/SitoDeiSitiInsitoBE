using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.Backend.External.SumUp.Models.SumUp
{
    public class HostedCheckoutSettings
    {
        public bool enabled { get; set; }

        public HostedCheckoutSettings()
        {
            enabled = true;
        }
    }

    public class HostedCheckoutInput
    {
        public decimal amount { get; set; }
        public string checkout_reference { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string merchant_code { get; set; }
        public HostedCheckoutSettings hosted_checkout_settings { get; set; }
        public HostedCheckoutInput()
        {
            hosted_checkout_settings = new();
        }
    }

    public class HostedCheckoutOutput : SumUpError
    {
        public int amount { get; set; }
        public string checkout_reference { get; set; }
        public string checkout_type { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public HostedCheckoutSettings hosted_checkout_settings { get; set; }
        public string hosted_checkout_url { get; set; }
        public string id { get; set; }
        public string merchant_code { get; set; }
        public string merchant_country { get; set; }
        public string merchant_name { get; set; }
        public string pay_to_email { get; set; }
        public string purpose { get; set; }
        public string status { get; set; }
        public List<object> transactions { get; set; }
    }
}
