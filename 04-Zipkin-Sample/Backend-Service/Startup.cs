using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zipkin.Library;

namespace Backend_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddZipkin();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);   
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseZipkin(configuration =>
            {
                configuration.SamplingRate = 1.0f;
                configuration.Url = "http://127.0.0.1:9411";
                configuration.ContentType = "application/json";
            });    
            
            app.UseMvc();

            
        }
    }
}
