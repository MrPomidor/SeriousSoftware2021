using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public class StockProfileResponse
    {
        public string Symbol { get; set; }
        public StockProfileResponseAssetProfile AssetProfile { get; set; }
    }

    public class StockProfileResponseAssetProfile
    {
        public string Phone { get; set; }
        public string LongBusinessSummary { get; set; }
    }
}
