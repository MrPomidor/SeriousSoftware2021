namespace SeriousBusiness.Stocks.DataStore
{
    public interface IStocksRepository
    {
        StockDataDto GetStocksFromWeek(string symbol, int week, int year);
        void SaveStocks(StockDataDto stockData, int week, int year);
    }
}
