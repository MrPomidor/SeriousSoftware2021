using System;
using System.Collections.Generic;

namespace SeriousBusiness.Stocks.DataProviders
{
    public class StockDataDto
    {
        public string Symbol { get; set; }

        public List<StockDataItemDto> Items { get; set; }
    }

    public class StockDataItemDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
