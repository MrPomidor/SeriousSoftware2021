using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public class YahooClient
    {
        private const string ApiKey = ""; // TODO fill
        private const string BaseUrl = "https://apidojo-yahoo-finance-v1.p.rapidapi.com";
        private const string Region = "US";
        private const string Lang = "en";
        private const string Host = "apidojo-yahoo-finance-v1.p.rapidapi.com";

        public async Task<StockChartsResponse> GetMonthDaylyStockChartsAsync(string symbol)
        {
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

        private HttpRequestMessage CreateGetRequest(string uri)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-key", ApiKey },
                    { "x-rapidapi-host", Host },
                },
            };
        }

        private async Task<T> GetResponseAsync<T>(HttpClient client, HttpRequestMessage request)
        {
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // TODO custom exception
            var body = await response.Content.ReadAsStringAsync();
            var responseDto = Deserialize<T>(body);
            return responseDto;
        }

        private T Deserialize<T>(string bodyStr)
        {
            throw new NotImplementedException();
        }

    }
}
