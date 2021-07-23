using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SeriousBusiness.Stocks.DataProviders.Yahoo;
using SeriousBusiness.Tests.TestUtils;
using SeriousBusiness.Utils;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using StockConsts = SeriousBusiness.Stocks.Consts;

namespace SeriousBusiness.Tests.Controllers
{
    public class StocksControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public StocksControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SymbolExists_ShouldReturn200()
        {
            const string symbol = "MSFT";

            var clientSubstitute = Substitute.For<IYahooClient>();
            clientSubstitute.GetStockProfile(symbol).Returns(new StockProfileResponse { AssetProfile = new StockProfileResponseAssetProfile { }, Symbol = symbol });
            clientSubstitute.GetMonthDaylyStockChartsAsync(symbol).Returns(GetCorrectGetStockChartsResponse());
            clientSubstitute.GetMonthDaylyStockChartsAsync(StockConsts.Symbols.SNP500).Returns(GetCorrectGetStockChartsResponse());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // override client to return pre-defined values
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IYahooClient));
                    services.Remove(descriptor);
                    services.AddSingleton<IYahooClient>((services) => clientSubstitute);
                });
            }).CreateClient();

            var response = await client.GetAsync($"/stocks/compareWithSpy/{symbol}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SymbolNotExist_ShouldReturn400()
        {
            const string symbol = "AAAAAAAAAA";

            var clientSubstitute = Substitute.For<IYahooClient>();
            clientSubstitute.GetStockProfile(symbol).Returns((StockProfileResponse)null);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // override client to return pre-defined values
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IYahooClient));
                    services.Remove(descriptor);
                    services.AddSingleton<IYahooClient>((services) => clientSubstitute);
                });
            }).CreateClient();

            var response = await client.GetAsync($"/stocks/compareWithSpy/{symbol}");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
            (await response.Content.ReadAsStringAsync()).ShouldBe("Symbol does not exist");
        }

        private StockChartsResponse GetCorrectGetStockChartsResponse()
        {
            const string filePath = "Stocks/DataProviders/Yahoo/GetStockChartsResponse.json";
            var contentJson = FileUtils.GetFileContentString(filePath);
            var obj = new JsonDeserializer().Deserialize<StockChartsResponse>(contentJson);
            return obj;
        }
    }
}
