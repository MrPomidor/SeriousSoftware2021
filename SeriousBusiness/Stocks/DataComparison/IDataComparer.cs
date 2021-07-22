using SeriousBusiness.Stocks.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataComparison
{
    public interface IDataComparer
    {
        CompareResultsDto Compare(StockDataDto stockDataRight, StockDataDto stockDataLeft);
    }
}
