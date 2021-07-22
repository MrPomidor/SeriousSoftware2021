using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public class YahooDataProvider : IDataProvider
    {
        public async Task<StockDataDto> GetWeekStockDataAsync(string symbol)
        {
            var client = new YahooClient();

            var clientDto = await client.GetMonthDaylyStockChartsAsync(symbol);
            var clientResultDto = clientDto.Chart.Result.Single();

            (var previousWeekFrom, var previousWeekTo) = GetPreviousWeekBoundaries();

            var results = new StockDataDto
            {
                Symbol = symbol,
                Items = new List<StockDataItemDto>()
            };

            for (int i = 0; i < clientResultDto.Timestamp.Length; i++)
            {
                var itemDate = FromEpoch(clientResultDto.Timestamp[i]);
                if (itemDate < previousWeekFrom || itemDate > previousWeekTo)
                    continue;

                var itemAdjclose = clientResultDto.Indicators.Adjclose.Single().Adjclose[i];
                results.Items.Add(new StockDataItemDto
                {
                    Date = itemDate,
                    Value = itemAdjclose
                });
            }

            return results;
        }

        private (DateTime weekFrom, DateTime weekTo) GetPreviousWeekBoundaries()
        {
            var dateNow = DateTime.Now; // TODO datetime provider
            var weekFrom = dateNow.AddDays(0 - Convert.ToInt16(dateNow.DayOfWeek) - 7);
            var weekTo = dateNow.AddDays(6 - Convert.ToInt16(dateNow.DayOfWeek) - 7);
            return (weekFrom, weekTo);
        }

        private DateTime FromEpoch(long epoch)
        {
            return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
        }

        public Task<bool> ValidateSymbolAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
