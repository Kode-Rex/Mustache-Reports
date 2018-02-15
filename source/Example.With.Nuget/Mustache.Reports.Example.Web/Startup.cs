using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Boundry.Report;
using Mustache.Reports.Data;
using Mustache.Reports.Domain;
using Swashbuckle.AspNetCore.Swagger;

namespace Mustache.Reports.Example.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterFrameworkDepenedencies(services);
            RegisterApplicationDepenedencies(services);

            RegisterSwagger(services);
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Mustache.Reports.Example", Version = "v1"});
            });
        }

        private void RegisterFrameworkDepenedencies(IServiceCollection services)
        {
            services.AddMvc();
        }

        private void RegisterApplicationDepenedencies(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IWordTemplaterGateway, WordTemplateGateway>();
            services.AddTransient<IPdfGateway, PdfGateway>();
            services.AddTransient<IRenderWordUseCase, RenderWordUseCase>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple.Report.Server");
            });
        }
    }
}
