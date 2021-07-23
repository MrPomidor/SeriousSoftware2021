using SeriousBusiness.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public class YahooDataProvider : IDataProvider
    {
        private readonly IYahooClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        public YahooDataProvider(
            IYahooClient client,
            IDateTimeProvider dateTimeProvider)
        {
            _client = client;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<StockDataDto> GetPreviousWeekStockDataAsync(string symbol)
        {
            var clientDto = await _client.GetMonthDaylyStockChartsAsync(symbol);

            var clientResultDto = clientDto?.Chart?.Result?.SingleOrDefault();
            var timestamps = clientResultDto?.Timestamp;
            var adjcloses = clientResultDto?.Indicators?.Adjclose?.SingleOrDefault()?.Adjclose;
            if (timestamps == null)
                throw new Exception($"Invalid response from client. Timestamps are null"); // TODO custom exception
            if (adjcloses == null)
                throw new Exception($"Invalid response from client. Adjcloses are null"); // TODO custom exception
            if (timestamps.Length != adjcloses.Length)
                throw new Exception($"Invalid response from client. Length of timestamps:{timestamps.Length} does not correspond to adjcloses:{adjcloses.Length}"); // TODO custom exception
            if (timestamps.Length == 0)
                throw new Exception($"Invalid response from client. No data available"); // TODO custom exception

            (var previousWeekFrom, var previousWeekTo) = GetPreviousWeekBoundaries();

            var results = new StockDataDto
            {
                Symbol = symbol,
                Items = new List<StockDataItemDto>()
            };

            for (int i = 0; i < timestamps.Length; i++)
            {
                var itemDate = FromEpoch(timestamps[i]);
                if (itemDate < previousWeekFrom || itemDate > previousWeekTo)
                    continue;

                var itemAdjclose = adjcloses[i];
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
            var dateNow = _dateTimeProvider.Now;
            var weekFrom = dateNow.AddDays(0 - Convert.ToInt16(dateNow.DayOfWeek) - 7);
            var weekTo = dateNow.AddDays(6 - Convert.ToInt16(dateNow.DayOfWeek) - 7);
            return (weekFrom, weekTo);
        }

        private DateTime FromEpoch(long epoch)
        {
            return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
        }

        public async Task<bool> ValidateSymbolAsync(string symbol)
        {
            var clientDto = await _client.GetStockProfile(symbol);
            return clientDto != null;
        }
    }
}
