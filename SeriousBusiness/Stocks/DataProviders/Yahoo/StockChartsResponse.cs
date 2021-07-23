namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public class StockChartsResponse
    {
        public StockChartsResponseChart Chart { get; set; }
    }

    public class StockChartsResponseChart
    {
        public StockChartsResponseChartResultItem[] Result { get; set; }
    }

    public class StockChartsResponseChartResultItem
    {
        /// <summary>
        /// Epoch timespampts
        /// </summary>
        public long[] Timestamp { get; set; }

        public StockChartsResponseChartResultItemIndicators Indicators { get; set; }
    }

    public class StockChartsResponseChartResultItemIndicators
    {
        public StockChartsResponseChartResultItemIndicatorsAdjclose[] Adjclose { get; set; }
    }

    public class StockChartsResponseChartResultItemIndicatorsAdjclose
    {
        public decimal[] Adjclose { get; set; }
    }
}
