using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeriousBusiness.Infrastructure;
using SeriousBusiness.Stocks.DataComparison;
using SeriousBusiness.Stocks.DataProviders;
using SeriousBusiness.Stocks.DataProviders.Yahoo;
using SeriousBusiness.Stocks.DataStore;
using SeriousBusiness.Utils;

namespace SeriousBusiness
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ErrorHandlingFilter));
            });

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IJsonDeserializer, JsonDeserializer>();

            services.AddTransient<IDataComparer, DefaultDataComparer>();
            services.AddTransient<IStocksRepository, LiteDbStocksRepository>();
            services.AddTransient<IDataProvider, YahooDataProvider>();
            services.AddTransient<IYahooClient, YahooClient>();
            services.AddTransient<IYahooClientConfiguration, YahooClientConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
