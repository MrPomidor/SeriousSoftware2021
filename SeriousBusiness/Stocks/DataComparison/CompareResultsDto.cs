using System;
using System.Collections.Generic;

namespace SeriousBusiness.Stocks.DataComparison
{
    public class CompareResultsDto
    {
        public List<CompareResultsDtoItem> Items { get; set; }
    }

    public class CompareResultsDtoItem
    {
        public DateTime Date { get; set; }
        public CompareResultsDtoItemSymbolData Symbol1Data { get; set; }
        public CompareResultsDtoItemSymbolData Symbol2Data { get; set; }
    }

    public class CompareResultsDtoItemSymbolData
    {
        public string Symbol { get; set; }
        public decimal Value { get; set; }
    }
}
