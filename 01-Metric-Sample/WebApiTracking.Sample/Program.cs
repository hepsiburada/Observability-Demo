using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApiTracking.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseMetrics()
                .ConfigureAppMetricsHostingConfiguration(options =>
                {
                    options.MetricsEndpoint = "/metrics";
                    options.MetricsEndpointPort = 5000;
                    options.MetricsTextEndpoint = "/metrics-text";
                    options.MetricsTextEndpointPort = 5000;
                })
                .UseStartup<Startup>();
        }
    }
}
