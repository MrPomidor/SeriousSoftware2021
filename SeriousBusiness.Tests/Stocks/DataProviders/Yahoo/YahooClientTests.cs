using NSubstitute;
using SeriousBusiness.Stocks.DataProviders.Yahoo;
using SeriousBusiness.Utils;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StockConsts = SeriousBusiness.Stocks.Consts;

namespace SeriousBusiness.Tests.Stocks.DataProviders.Yahoo
{
    public class YahooClientTests
    {
        private readonly YahooClient client;
        public YahooClientTests()
        {
            var configuration = Substitute.For<IYahooClientConfiguration>();
            configuration.ApiKey.Returns("INPUT YOUR API KEY HERE");
            client = new YahooClient(new JsonDeserializer(), configuration);
        }

        [Theory(Skip = "API key should be configured in YahooClient class to run this tests")]
        [InlineData("MSFT")]
        [InlineData(StockConsts.Symbols.SNP500)]
        public async Task GetMonthDaylyStockChartsAsync_SymbolsCorrect_ShouldNotThrow(string symbol)
        {
            var clientResponse = await client.GetMonthDaylyStockChartsAsync(symbol);
            clientResponse.ShouldNotBeNull();
            (clientResponse?.Chart?.Result).ShouldHaveSingleItem();
            var timespampLength = clientResponse.Chart.Result.Single().Timestamp?.Length;
            (clientResponse.Chart.Result.Single().Indicators?.Adjclose).ShouldHaveSingleItem();
            var adjcloseLength = clientResponse.Chart.Result.Single().Indicators.Adjclose.Single().Adjclose?.Length;
            timespampLength.ShouldNotBeNull();
            adjcloseLength.ShouldNotBeNull();
            timespampLength.ShouldBe(adjcloseLength);
        }

        [Theory(Skip = "API key should be configured in YahooClient class to run this tests")]
        [InlineData("MSFT")]
        public async Task GetStockProfile_SymbolsCorrect_ShouldNotThrow(string symbol)
        {
            var clientResponse = await client.GetStockProfile(symbol);
            clientResponse.ShouldNotBeNull();
            clientResponse.Symbol.ShouldNotBeNull();
            clientResponse.AssetProfile.ShouldNotBeNull();
        }

        [Theory(Skip = "API key should be configured in YahooClient class to run this tests")]
        [InlineData("AAAAAAAAAAA")]
        public async Task GetStockProfile_SymbolsIncorrect_ShouldReturnNull(string symbol)
        {
            var clientResponse = await client.GetStockProfile(symbol);
            clientResponse.ShouldBeNull();
        }
    }
}
