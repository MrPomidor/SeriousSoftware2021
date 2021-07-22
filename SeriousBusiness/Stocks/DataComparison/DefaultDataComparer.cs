using SeriousBusiness.Stocks.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeriousBusiness.Stocks.DataComparison
{
    public class DefaultDataComparer : IDataComparer
    {
        public CompareResultsDto Compare(StockDataDto stockDataRight, StockDataDto stockDataLeft)
        {
            if (stockDataRight.Items.Count != stockDataLeft.Items.Count)
                throw new ArgumentException($"Amount of items in stockDataRight and stockDataLeft are not the same. Right:{stockDataRight.Items.Count} vs Left{stockDataLeft.Items.Count}");

            var rightData = stockDataRight.Items.OrderBy(x => x.Date).ToArray();
            var leftData = stockDataLeft.Items.OrderBy(x => x.Date).ToArray();
            var rightCompareDataRaw = new decimal[rightData.Length];
            var leftCompareDataRaw = new decimal[leftData.Length];

            FillCompareDataRow(rightData, rightCompareDataRaw);
            FillCompareDataRow(leftData, leftCompareDataRaw);

            var results = new CompareResultsDto
            {
                Items = new List<CompareResultsDtoItem>()
            };
            
            for(int i = 0; i < rightData.Length; i++)
            {
                var dataRight = rightData[i];
                var dataLeft = leftData[i];
                if (dataRight.Date != dataLeft.Date)
                    throw new ArgumentException($"There is no correspondance of {dataRight.Date} in the left symbol {stockDataLeft.Symbol} data");

                var compareItem = new CompareResultsDtoItem
                {
                    Date = dataRight.Date,
                    Symbol1Data = new CompareResultsDtoItemSymbolData
                    {
                        Symbol = stockDataRight.Symbol,
                        Value = dataRight.Value
                    },
                    Symbol2Data = new CompareResultsDtoItemSymbolData
                    {
                        Symbol = stockDataLeft.Symbol,
                        Value = dataLeft.Value
                    }
                };
                results.Items.Add(compareItem);
            }

            return results;
        }

        private void FillCompareDataRow(StockDataItemDto[] dataItems, decimal[] performanceItems)
        {
            var initialValue = dataItems[0].Value;
            for (int i = 0; i < dataItems.Length; i++)
            {
                var performace = (dataItems[i].Value / initialValue * 100) - 100;
                performanceItems[i] = performace;
            }
        }
    }
}
