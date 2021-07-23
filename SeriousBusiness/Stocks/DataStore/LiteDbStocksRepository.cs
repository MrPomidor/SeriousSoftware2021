using LiteDB;

namespace SeriousBusiness.Stocks.DataStore
{
    public class LiteDbStocksRepository : IStocksRepository
    {
        public StockDataDto GetStocksFromWeek(string symbol, int week, int year)
        {
            var key = CreateKey(symbol, week, year);

            using var db = GetDb();
            var collection = db.GetCollection<StockDataDto>();
            var entity = collection.FindById(key);

            return entity;
        }

        public void SaveStocks(StockDataDto stockData, int week, int year)
        {
            var key = CreateKey(stockData.Symbol, week, year);

            using var db = GetDb();
            var collection = db.GetCollection<StockDataDto>();
            collection.Insert(key, stockData);
        }

        private LiteDatabase GetDb() => new LiteDatabase(":memory:");

        private string CreateKey(string symbol, int week, int year) => $"{symbol}-{week}-{year}";
    }
}
