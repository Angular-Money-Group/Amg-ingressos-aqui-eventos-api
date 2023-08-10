using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public class CieloClient : ICieloClient
    {
        private readonly HttpClient _httpClient;
        private IOptions<CieloSettings> _config;
        public CieloClient(IOptions<CieloSettings> transactionDatabaseSettings, HttpClient httpClientFactory)
        {
            _config = transactionDatabaseSettings;
            _httpClient = httpClientFactory;
        }

        public HttpClient CreateClient()
        {
            _httpClient.BaseAddress = new Uri(_config.Value.UrlApiHomolog);
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/vnd.github.v3+json");
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.UserAgent, "HttpRequestsSample");
            _httpClient.DefaultRequestHeaders.Add(
                "MerchantId", _config.Value.MerchantIdHomolog);
            _httpClient.DefaultRequestHeaders.Add(
                "MerchantKey", _config.Value.MerchantKeyHomolog);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

            return _httpClient;
        }
    }
}