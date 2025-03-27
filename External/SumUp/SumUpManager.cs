using Microsoft.Extensions.Options;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.External.SumUp.Interfaces;
using SitoDeiSiti.External.SumUp.Models.SumUp;
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

                SumUpHttpClient.BaseAddress = new Uri(options.Value.SumUpCheckoutUrl);

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
    }
}
