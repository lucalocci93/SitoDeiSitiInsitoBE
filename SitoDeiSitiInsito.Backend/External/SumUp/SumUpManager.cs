using Microsoft.Extensions.Options;
using SitoDeiSiti.Backend.External.SumUp.Models.SumUp;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.External.SumUp.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SitoDeiSiti.External.SumUp
{
    public class SumUpManager : ISumUpManager
    {
        private readonly HttpClient SumUpHttpClient;
        private readonly IOptions<SitoDeiSiti.DTOs.SumUp> options;

        public SumUpManager(HttpClient httpClient, IOptions<SitoDeiSiti.DTOs.SumUp> opt)
        {
            SumUpHttpClient = httpClient;
            options = opt;
        }

        public async Task<HostedCheckoutOutput> CreateHostedCheckout(HostedCheckoutInput input)
        {
            try
            {
                input.merchant_code = options.Value.MerchantCode;

                string json = JsonSerializer.Serialize(input);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Content = content;
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                SumUpHttpClient.BaseAddress = new Uri(string.Concat(options.Value.SumUpCheckoutUrl, "checkouts"));

                HttpResponseMessage response = await SumUpHttpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HostedCheckoutOutput hostedCheckoutOutput = JsonSerializer.Deserialize<HostedCheckoutOutput>(responseString)!;
                    return hostedCheckoutOutput;
                }
                else
                {
                    string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception("Errore creazione hosted checkout");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SumUpListCheckoutOutput>> GetSumUpCheckoutList()
        {
            List<SumUpListCheckoutOutput> checkouts = new List<SumUpListCheckoutOutput>();
            string responseString = string.Empty;
            try 
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;

                SumUpHttpClient.BaseAddress = new Uri(string.Concat(options.Value.SumUpCheckoutUrl, "checkouts"));

                HttpResponseMessage response = await SumUpHttpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                return JsonSerializer.Deserialize<List<SumUpListCheckoutOutput>>(responseString) ?? new List<SumUpListCheckoutOutput>();
            }
            catch (Exception ex)
            {
                checkouts.Add(new SumUpListCheckoutOutput
                {
                    error_code = "-404",
                });

                return checkouts;
            }
        }
    }
}
