using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace webApi
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
                .UseMetricsWebTracking()
                .UseMetrics(options =>
                {
                    //prometheus reporter
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter =
                            new MetricsPrometheusTextOutputFormatter();
                        endpointsOptions.MetricsEndpointOutputFormatter =
                            new MetricsPrometheusProtobufOutputFormatter();
                    };
                })
                .ConfigureAppMetricsHostingConfiguration(options =>
                {
                    options.MetricsEndpoint = "/metrics";
                    options.MetricsEndpointPort = 3333;
                    options.MetricsTextEndpoint = "/metrics-text";
                    options.MetricsTextEndpointPort = 3333;
                })
                .UseStartup<Startup>();
        }
    }
}
