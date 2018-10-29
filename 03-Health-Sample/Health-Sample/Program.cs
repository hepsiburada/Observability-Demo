using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;

namespace Health_Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureHealthWithDefaults(
                    builder =>
                    {
                        const int threshold = 100;
                        builder.HealthChecks.AddCheck("DatabaseConnected",
                            () => new ValueTask<HealthCheckResult>(
                                HealthCheckResult.Healthy("Database Connection OK")));
                        builder.HealthChecks.AddProcessPrivateMemorySizeCheck("Private Memory Size", threshold);
                        builder.HealthChecks.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", threshold);
                        builder.HealthChecks.AddProcessPhysicalMemoryCheck("Working Set", threshold);
                        builder.HealthChecks.AddPingCheck("service ping", "rabbitmq.hb.com", TimeSpan.FromSeconds(10));
                        builder.HealthChecks.AddHttpGetCheck("hb", new Uri("https://hepsiburada.com/"),
                                TimeSpan.FromSeconds(10));
                        /*
                         SQL bağlantı kontrolü
                        builder.HealthChecks.AddSqlCachedCheck("DB Connection Cached",
                            () => new SqliteConnection(ConnectionString),
                            TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));
                        */
                        //Slack entegrasyonu
                        builder.Report.ToSlack(
                            options =>
                            {
                                options.Channel = "#alarm";
                                options.WebhookUrl = "http://test.slack.com?webhook";
                                options.Enabled = true;
                            });

                    })
                    .ConfigureAppHealthHostingConfiguration(options =>
                    {
                        // options.AllEndpointsPort = 3333;
                        options.HealthEndpoint = "/my-health";
                        options.HealthEndpointPort = 1111;
                        options.PingEndpoint = "/my-ping";
                        options.PingEndpointPort = 2222;
                    })
                    .UseHealth()
                    .UseStartup<Startup>();
    }
}
