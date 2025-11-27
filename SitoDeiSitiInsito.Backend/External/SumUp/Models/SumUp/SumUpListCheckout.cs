namespace SitoDeiSiti.Backend.External.SumUp.Models.SumUp
{
    public class Mandate
    {
        public string? type { get; set; }
        public string? status { get; set; }
        public string? merchant_code { get; set; }
    }

    public class PaymentInstrument
    {
        public string? token { get; set; }
    }

    public class SumUpListCheckoutOutput : SumUpError
    {
        public object? checkout_reference { get; set; }
        public double amount { get; set; }
        public string? currency { get; set; }
        public string? merchant_code { get; set; }
        public string? description { get; set; }
        public string? return_url { get; set; }
        public string? id { get; set; }
        public object? status { get; set; }
        public DateTime date { get; set; }
        public DateTime valid_until { get; set; }
        public string? customer_id { get; set; }
        public Mandate? mandate { get; set; }
        public List<Transaction>? transactions { get; set; }
        public string? transaction_code { get; set; }
        public string? transaction_id { get; set; }
        public string? merchant_name { get; set; }
        public string? redirect_url { get; set; }
        public PaymentInstrument? payment_instrument { get; set; }
    }

    public class Transaction
    {
        public string? id { get; set; }
        public string? transaction_code { get; set; }
        public double amount { get; set; }
        public string? currency { get; set; }
        public DateTime timestamp { get; set; }
        public object? status { get; set; }
        public object? payment_type { get; set; }
        public object? installments_count { get; set; }
        public string merchant_code { get; set; }
        public int vat_amount { get; set; }
        public int tip_amount { get; set; }
        public object? entry_mode { get; set; }
        public string? auth_code { get; set; }
        public int internal_id { get; set; }
    }


}
