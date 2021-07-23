using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders
{
    public interface IDataProvider
    {
        Task<bool> SymbolExistsAsync(string symbol);
        Task<StockDataDto> GetPreviousWeekStockDataAsync(string symbol);
    }
}
