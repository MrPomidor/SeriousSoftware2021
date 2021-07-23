using SeriousBusiness.Stocks;
using SeriousBusiness.Stocks.DataStore;
using Shouldly;
using Xunit;

namespace SeriousBusiness.Tests.Stocks.DataStore
{
    public class LiteDbStocksRepositoryTests
    {
        private readonly LiteDbStocksRepository repository;
        public LiteDbStocksRepositoryTests()
        {
            repository = new LiteDbStocksRepository();
        }

        [Fact]
        public void ItemNotExist_ShouldReturnNull()
        {
            var symbol = "AAA";
            var week = 1;
            var year = 2021;

            var result = repository.GetStocksFromWeek(symbol, week, year);
            result.ShouldBeNull();
        }

        [Fact]
        public void ItemExist_ShouldReturnItem()
        {
            var week = 1;
            var year = 2021;
            var item = new StockDataDto
            {
                Symbol = "AAA"
            };

            repository.SaveStocks(item, week, year);

            var result = repository.GetStocksFromWeek(item.Symbol, week, year);
            result.ShouldBeNull();
        }
    }
}
