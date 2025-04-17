using Azure.Core;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using SitoDeiSiti.Utils.HTTPHandlers.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SitoDeiSiti.Utils.HTTPHandlers
{
    public class OAuth2HttpHandler : DelegatingHandler
    {
        private readonly string AuthUrl;
        private readonly string GrantType;
        private readonly string ClientId;
        private readonly string ClientSecret;

        public OAuth2HttpHandler(string authUrl, string grantType, string clientId, string clientSecret)
        {
            AuthUrl = authUrl;
            GrantType = grantType;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string JwtToken = await GetToken(AuthUrl, GrantType, ClientId, ClientSecret).ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(true);

            return response;
        }

        private async Task<string> GetToken(string AuthUrl, string GrantType, string ClientId, string ClientSecret)
        {
            Token response = new();
            string CachedValue = string.Empty;

            try
            {
                var values = new Dictionary<string, string>
                {
                    { "grant_type", GrantType },
                    { "client_id", ClientId },
                    { "client_secret", ClientSecret }
                };

                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();

                    request.Method = HttpMethod.Post;
                    request.Content = content;

                    HttpClient httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(AuthUrl);

                    HttpResponseMessage resp = await httpClient.SendAsync(request);
                    resp.EnsureSuccessStatusCode();

                    string responseBody = await resp.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        response = JsonSerializer.Deserialize<Token>(responseBody)!;
                    }
                }
                catch (HttpRequestException e)
                {
                    throw new Exception(e.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return response.access_token;

        }

    }
}
