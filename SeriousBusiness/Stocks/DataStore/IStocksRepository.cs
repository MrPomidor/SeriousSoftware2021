using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataStore
{
    public interface IStocksRepository
    {
        StockDataDto GetStocksFromWeek(string symbol, int week, int year);
        void SaveStocks(StockDataDto stockData, int week, int year);
    }
}
