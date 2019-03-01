using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Boundary.Options;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Word;
using Mustache.Reports.Data;
using Mustache.Reports.Domain;
using Mustache.Reports.Domain.Pdf;
using Mustache.Reports.Domain.Word;
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
            Register_Mvc(services);
            Register_Application_Dependencies(services);
            Register_Configuration(services);
            RegisterSwagger(services);
        }

        private void Register_Configuration(IServiceCollection services)
        {
            services.Configure<MustacheReportOptions>(Configuration.GetSection("MustacheReportOptions"));
            services.Configure<DemoOptions>(Configuration.GetSection("DemoOptions"));
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mustache.Reports");
            });
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Mustache.Reports.Example", Version = "v1" });
            });
        }

        private void Register_Mvc(IServiceCollection services)
        {
            services.AddMvc();
        }

        private void Register_Application_Dependencies(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IReportGateway, ReportGateway>();
            services.AddTransient<IPdfGateway, PdfGateway>();
            services.AddTransient<IRenderWordUseCase, RenderWordUseCase>();
            services.AddTransient<IRenderDocxToPdfUseCase, RenderWordToPdfUseCase>();
            services.AddTransient<IRenderAsWordThenPdfUseCase, RenderAsWordThenPdfUseCase>();
        }
    }
}
