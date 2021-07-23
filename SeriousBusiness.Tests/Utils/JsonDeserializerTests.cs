using SeriousBusiness.Stocks.DataProviders.Yahoo;
using SeriousBusiness.Tests.TestUtils;
using SeriousBusiness.Utils;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace SeriousBusiness.Tests.Utils
{
    public class JsonDeserializerTests
    {
        private readonly JsonDeserializer deserializer;
        public JsonDeserializerTests()
        {
            deserializer = new JsonDeserializer();
        }

        [Fact]
        public void ShouldDeserializeCorrectly_StockChartsResponse()
        {
            const string filePath = "Stocks/DataProviders/Yahoo/GetStockChartsResponse.json";

            var contentJson = FileUtils.GetFileContentString(filePath);

            var obj = deserializer.Deserialize<StockChartsResponse>(contentJson);

            obj.ShouldNotBeNull();
            (obj.Chart?.Result?.Length).ShouldBe(1);
            (obj.Chart?.Result.Single().Timestamp?.Length).ShouldBe(21);
            (obj.Chart?.Result.Single().Indicators?.Adjclose?.SingleOrDefault()?.Adjclose?.Length).ShouldBe(21);
        }
    }
}
