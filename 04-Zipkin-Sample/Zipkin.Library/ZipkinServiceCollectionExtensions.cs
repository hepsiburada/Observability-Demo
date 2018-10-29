using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace Zipkin.Library
{
    public static class ZipkinServiceCollectionExtensions
    {
        public static IServiceCollection AddZipkin(this IServiceCollection services)
        {
            services.TryAddSingleton<ITraceFactory,TraceFactory>();
            
            return services;
        }

        public static IServiceCollection AddZipkin(this IServiceCollection services, Action<ZipkinConfiguration> configuration, ILoggerFactory factory)
        {
            services.TryAddSingleton<ITraceFactory, TraceFactory>();
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var config = new ZipkinConfiguration();
            configuration(config);

            TraceManager.SamplingRate = config.SamplingRate;
            var logger = new TracingLogger(factory, "zipkin4net");
            var httpSender = new HttpZipkinSender(config.Url, config.ContentType);
            var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer());
            TraceManager.RegisterTracer(tracer);
            TraceManager.Start(logger);

            return services;
        }
    }
}