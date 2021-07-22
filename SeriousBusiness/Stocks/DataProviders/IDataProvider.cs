using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders
{
    public interface IDataProvider
    {
        Task<bool> ValidateSymbolAsync(string symbol);
        Task<StockDataDto> GetWeekStockDataAsync(string symbol);
    }
}
