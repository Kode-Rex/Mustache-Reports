using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simple.Report.Server.Data.Repositories;
using Simple.Report.Server.Domain.Repositories;
using Simple.Report.Server.Domain.UseCases;
using Simple.Report.Server.UseCase;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace Simple.Report.Server.Api.Example
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private readonly Container _container = new Container();

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
            // Add framework services.
            services.AddMvc();

            RegisterSimpleInjector(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Simple.Report.Server.Api.Example", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        private void RegisterSimpleInjector(IServiceCollection services)
        {
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));

            services.UseSimpleInjectorAspNetRequestScoping(_container);

            RegisterUseCases();
            RegisterRepositories(services);
        }

        private void RegisterUseCases()
        {
            _container.Register<IRenderReportUseCase, RenderReportUseCase>();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            var webRoot = GetWebRootPath(services);

            // todo : Path.Combine not used cuz it breaks if there are spaces in folder name ;(
            var templateLocation = webRoot + "\\Reporting\\Templates";
            var nodeAppLocation = webRoot + "\\Reporting\\NodeApp";

            _container.Register<IReportRepository>(() => new ReportRepository(templateLocation, nodeAppLocation));
        }

        private string GetWebRootPath(IServiceCollection services)
        {
            var serviceDescriptor = services.First(c => c.ServiceType == typeof(IHostingEnvironment));
            var hostingEnvironment = (IHostingEnvironment)serviceDescriptor.ImplementationInstance;
            return IsWebRootPathNull(hostingEnvironment) ? hostingEnvironment.ContentRootPath : hostingEnvironment.WebRootPath;
        }

        private static bool IsWebRootPathNull(IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.WebRootPath == null;
        }
    }
}
