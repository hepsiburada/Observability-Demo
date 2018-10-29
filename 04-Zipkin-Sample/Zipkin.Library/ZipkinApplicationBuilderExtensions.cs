using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Propagation;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace Zipkin.Library
{

    public static class ZipkinApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseZipkin(this IApplicationBuilder app,
            Action<ZipkinConfiguration> configureAction)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            if (lifetime == null) throw new ArgumentNullException(nameof(lifetime));
            
            var config = new ZipkinConfiguration();
            configureAction(config);
            
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = config.SamplingRate;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender(config.Url, config.ContentType);
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer());
                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
            });
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing($"{AppDomain.CurrentDomain.FriendlyName}");
            
            
            return app;
        }
    }
}