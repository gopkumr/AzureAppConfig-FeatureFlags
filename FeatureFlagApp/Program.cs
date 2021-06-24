using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;

namespace FeatureFlagApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config => {
                        var settings = config.Build();
                        var connection = settings.GetConnectionString("AppConfiguration");
                        config.AddAzureAppConfiguration(option => option.Connect(connection)
                                                                        .UseFeatureFlags()
                                                                        .ConfigureRefresh(refresh =>
                                                                        {
                                                                            refresh.Register("RefreshKey", true).SetCacheExpiration(new TimeSpan(0, 0, 5));
                                                                        })

                        );
                    }).UseStartup<Startup>();
                });
    }
}
