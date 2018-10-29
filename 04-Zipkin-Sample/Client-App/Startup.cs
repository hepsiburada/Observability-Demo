using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport;
using zipkin4net.Transport.Http;
using zipkin4net.Middleware;
using System.Net.Http;
using System.Threading;
using Zipkin.Library;

namespace Client_App
{
    public class Startup 
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public  void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("Tracer").AddHttpMessageHandler(provider => TracingHandler.WithoutInnerHandler("Client-App"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           
            app.UseZipkin(configuration =>
            {
                configuration.SamplingRate = 1.0f;
                configuration.Url = "http://127.0.0.1:9411";
                configuration.ContentType = "application/json";
            });  
            
            
            app.Run(async (context) =>
            {
                var callServiceUrl = "http://127.0.0.1:5000/api/values";
                var clientFactory = app.ApplicationServices.GetService<IHttpClientFactory>();
                using (var httpClient = clientFactory.CreateClient("Tracer"))
                {
                    var response = await httpClient.GetAsync(callServiceUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        await context.Response.WriteAsync(response.ReasonPhrase);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        await context.Response.WriteAsync(content);
                    }
                }
            });
        }
    }
}