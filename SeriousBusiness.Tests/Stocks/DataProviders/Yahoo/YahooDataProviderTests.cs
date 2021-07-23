using NSubstitute;
using SeriousBusiness.Stocks.DataProviders.Yahoo;
using SeriousBusiness.Tests.TestUtils;
using SeriousBusiness.Utils;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SeriousBusiness.Tests.Stocks.DataProviders.Yahoo
{
    public class YahooDataProviderTests
    {
        private readonly IYahooClient clientSubstitute;
        private readonly IDateTimeProvider dateTimeProviderSubstitute;
        private readonly YahooDataProvider dataProvider;
        public YahooDataProviderTests()
        {
            clientSubstitute = Substitute.For<IYahooClient>();
            dateTimeProviderSubstitute = Substitute.For<IDateTimeProvider>();
            dataProvider = new YahooDataProvider(clientSubstitute, dateTimeProviderSubstitute);
        }

        [Fact]
        public async Task GetWeekStockDataAsync_ClientResponseCorrect_ShouldReturnCorrectStockDataDto()
        {
            var symbol = "AAA";

            clientSubstitute.GetMonthDaylyStockChartsAsync(symbol).Returns(GetCorrectGetStockChartsResponse());
            dateTimeProviderSubstitute.Now.Returns(new DateTime(2021, 7, 21));

            var results = await dataProvider.GetWeekStockDataAsync(symbol);
            results.ShouldNotBeNull();
            results.Symbol.ShouldBe(symbol);
            (results.Items?.Count).ShouldBe(5); // last week except weekends
            results.Items[0].Date.Date.ShouldBe(new DateTime(2021, 7, 12));
            results.Items[0].Value.ShouldBe(277.32000732421875m);
            results.Items[4].Date.Date.ShouldBe(new DateTime(2021, 7, 16));
            results.Items[4].Value.ShouldBe(280.75m);
        }

        // returns real response with dates from June 22, 2021 to July 21, 2021
        private StockChartsResponse GetCorrectGetStockChartsResponse()
        {
            const string filePath = "Stocks/DataProviders/Yahoo/GetStockChartsResponse.json";
            var contentJson = FileUtils.GetFileContentString(filePath);
            var obj = new JsonDeserializer().Deserialize<StockChartsResponse>(contentJson);
            return obj;
        }
    }
}
