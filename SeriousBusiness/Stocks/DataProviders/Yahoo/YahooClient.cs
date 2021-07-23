using SeriousBusiness.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public interface IYahooClient
    {
        Task<StockChartsResponse> GetMonthDaylyStockChartsAsync(string symbol);
        Task<StockProfileResponse> GetStockProfile(string symbol);
    }

    public class YahooClient : IYahooClient
    {
        private const string BaseUrl = "https://apidojo-yahoo-finance-v1.p.rapidapi.com";
        private const string Region = "US";
        private const string Host = "apidojo-yahoo-finance-v1.p.rapidapi.com";

        private readonly IJsonDeserializer _jsonDeserializer;
        private readonly IYahooClientConfiguration _configuration;
        public YahooClient(
            IJsonDeserializer jsonDeserializer,
            IYahooClientConfiguration configuration
            )
        {
            _jsonDeserializer = jsonDeserializer;
            _configuration = configuration;
        }

        public async Task<StockChartsResponse> GetMonthDaylyStockChartsAsync(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException("Symbol cannot be empty", nameof(symbol));

            const string interval = "1d";
            const string range = "1mo";
            var uri = $"{BaseUrl}/stock/v2/get-chart?" +
                $"interval={interval}&" + 
                $"symbol={symbol}&" + 
                $"range={range}&" + 
                $"region={Region}";
            var request = CreateGetRequest(uri);

            using var client = new HttpClient();
            var response = await GetResponseAsync<StockChartsResponse>(client, request);
            return response;
        }

        public async Task<StockProfileResponse> GetStockProfile(string symbol)
        {
            var uri = $"{BaseUrl}/stock/v2/get-profile?" +
                $"symbol={symbol}&" +
                $"region={Region}";
            var request = CreateGetRequest(uri);

            using var client = new HttpClient();
            var response = await GetResponseAsync<StockProfileResponse>(client, request);
            response = response?.Symbol == null && response?.AssetProfile == null ? null : response;
            return response;
        }

        private HttpRequestMessage CreateGetRequest(string uri)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-key", _configuration.ApiKey },
                    { "x-rapidapi-host", Host },
                },
            };
        }

        private async Task<T> GetResponseAsync<T>(HttpClient client, HttpRequestMessage request)
        {
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // TODO custom exception
            var body = await response.Content.ReadAsStringAsync();
            var responseDto = _jsonDeserializer.Deserialize<T>(body);
            return responseDto;
        }

    }
}
