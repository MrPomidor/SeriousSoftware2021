namespace SeriousBusiness.Stocks.DataComparison
{
    public interface IDataComparer
    {
        CompareResultsDto Compare(StockDataDto stockDataRight, StockDataDto stockDataLeft);
    }
}
