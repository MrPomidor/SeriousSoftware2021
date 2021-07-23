using SeriousBusiness.Stocks.DataComparison;
using SeriousBusiness.Stocks.DataProviders;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeriousBusiness.Tests.Stocks.DataComparison
{
    public class DefaultDataComparerTests
    {
        private readonly DefaultDataComparer dataComparer;
        public DefaultDataComparerTests()
        {
            dataComparer = new DefaultDataComparer();
        }

        [Fact]
        public void ShouldGetCorrectComparison()
        {
            var stockData1 = new StockDataDto
            {
                Symbol = "AAA",
                Items = new List<StockDataItemDto>
                {
                    new StockDataItemDto { Date = new DateTime(2021, 1, 1), Value = 100 },
                    new StockDataItemDto { Date = new DateTime(2021, 1, 2), Value = 80 },
                    new StockDataItemDto { Date = new DateTime(2021, 1, 3), Value = 190 }
                }
            };
            var stockData2 = new StockDataDto
            {
                Symbol = "BBB",
                Items = new List<StockDataItemDto>
                {
                    new StockDataItemDto { Date = new DateTime(2021, 1, 1), Value = 200 },
                    new StockDataItemDto { Date = new DateTime(2021, 1, 2), Value = 240 },
                    new StockDataItemDto { Date = new DateTime(2021, 1, 3), Value = 180 },
                }
            };

            var compareResults = dataComparer.Compare(stockData1, stockData2);

            // Let's say for stock XYZ, the first day price is 100, and the second day price is 120 and third day price is 110. So the performance data would be 0%, 20% ,10%.
            compareResults.ShouldNotBeNull();
            (compareResults.Items?.Count).ShouldBe(3);
            compareResults.Items[0].Date.Date.ShouldBe(new DateTime(2021, 1, 1));
            compareResults.Items[0].Symbol1Data.Symbol.ShouldBe("AAA");
            compareResults.Items[0].Symbol1Data.Value.ShouldBe(0);
            compareResults.Items[0].Symbol2Data.Symbol.ShouldBe("BBB");
            compareResults.Items[0].Symbol2Data.Value.ShouldBe(0);
            compareResults.Items[1].Date.Date.ShouldBe(new DateTime(2021, 1, 2));
            compareResults.Items[1].Symbol1Data.Symbol.ShouldBe("AAA");
            compareResults.Items[1].Symbol1Data.Value.ShouldBe(-20);
            compareResults.Items[1].Symbol2Data.Symbol.ShouldBe("BBB");
            compareResults.Items[1].Symbol2Data.Value.ShouldBe(20);
            compareResults.Items[2].Date.Date.ShouldBe(new DateTime(2021, 1, 3));
            compareResults.Items[2].Symbol1Data.Symbol.ShouldBe("AAA");
            compareResults.Items[2].Symbol1Data.Value.ShouldBe(90);
            compareResults.Items[2].Symbol2Data.Symbol.ShouldBe("BBB");
            compareResults.Items[2].Symbol2Data.Value.ShouldBe(-10);
        }
    }
}
