using Microsoft.AspNetCore.Mvc;
using SeriousBusiness.Stocks.DataComparison;
using SeriousBusiness.Stocks.DataProviders;
using System;
using System.Collections.Generic;
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
        public StocksController(
            IDataProvider dataProvider,
            IDataComparer dataComparer
            )
        {
            _dataProvider = dataProvider;
            _dataComparer = dataComparer;
        }

        // TODO exception processing filter

        [HttpGet("/compareWithSpy/{symbol}")]
        public async Task<CompareResultsDto> CompareWithSPY(string symbol)
        {
            // TODO validate symbol
            //if (!await _dataProvider.ValidateSymbolAsync(symbol))
            //{
            //    throw new Exception("invalid request !"); // TODO special exception type
            //}

            var symbolData = await _dataProvider.GetWeekStockDataAsync(symbol);

            // load data from last week
            // save/update data in db
            // get data for SPY
            var spyData = await _dataProvider.GetWeekStockDataAsync(StockConsts.Symbols.SNP500);
            // compare using comparer
            var compareResults = _dataComparer.Compare(symbolData, spyData);
            return compareResults;
        }
    }
}
