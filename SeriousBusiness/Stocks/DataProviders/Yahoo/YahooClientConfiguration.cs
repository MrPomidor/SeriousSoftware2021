using Microsoft.Extensions.Configuration;

namespace SeriousBusiness.Stocks.DataProviders.Yahoo
{
    public interface IYahooClientConfiguration
    {
        string ApiKey { get; }
    }

    public class YahooClientConfiguration : IYahooClientConfiguration
    {
        private readonly IConfiguration _configuration;
        public YahooClientConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ApiKey { get => _configuration["Yahoo:ApiKey"]; }
    }
}
