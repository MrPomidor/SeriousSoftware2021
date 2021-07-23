using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeriousBusiness.Infrastructure;
using SeriousBusiness.Stocks;
using SeriousBusiness.Stocks.DataComparison;
using SeriousBusiness.Stocks.DataProviders;
using SeriousBusiness.Stocks.DataStore;
using SeriousBusiness.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using StockConsts = SeriousBusiness.Stocks.Consts;

namespace SeriousBusiness.Controllers
{
    [ApiController]
    [Route("stocks")]
    public class StocksController : ControllerBase
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataComparer _dataComparer;
        private readonly IStocksRepository _stocksRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        public StocksController(
            IDataProvider dataProvider,
            IDataComparer dataComparer,
            IStocksRepository stocksRepository,
            IDateTimeProvider dateTimeProvider
            )
        {
            _dataProvider = dataProvider;
            _dataComparer = dataComparer;
            _stocksRepository = stocksRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        [AllowAnonymous]
        [HttpGet("compareWithSpy/{symbol}")]
        public async Task<CompareResultsDto> CompareWithSPY(string symbol)
        {
            // validate symbol
            if (string.IsNullOrEmpty(symbol))
                throw new InvalidInputException("Symbol could not be empty");

            if (!await _dataProvider.SymbolExistsAsync(symbol))
                throw new InvalidInputException("Symbol does not exist");

            // load data from last week
            var symbolData = await GetSymbolStocks(symbol);
            var spySymbolData = await GetSymbolStocks(StockConsts.Symbols.SNP500);

            var compareResults = _dataComparer.Compare(symbolData, spySymbolData);
            return compareResults;
        }

        private async Task<StockDataDto> GetSymbolStocks(string symbol)
        {
            (var week, var year) = GetLastWeekInfo();
            var stocks = _stocksRepository.GetStocksFromWeek(symbol, week, year);
            if (stocks != null)
                return stocks;

            var symbolData = await _dataProvider.GetPreviousWeekStockDataAsync(symbol);
            _stocksRepository.SaveStocks(symbolData, week, year);
            return symbolData;
        }

        private (int week, int year) GetLastWeekInfo()
        {
            var dateNow = _dateTimeProvider.Now;
            var prevWeek = dateNow.AddDays(0 - Convert.ToInt16(dateNow.DayOfWeek) - 7);

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            var weekNumber = cal.GetWeekOfYear(prevWeek, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var year = prevWeek.Year;
            return (weekNumber, year);
        }
    }
}
