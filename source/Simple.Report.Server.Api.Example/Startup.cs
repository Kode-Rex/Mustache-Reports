using System.IO;
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
using SimpleInjector.Lifestyles;
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
            services.AddMvc();

            RegisterSimpleInjector(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Simple.Report.Server.Example", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeContainer(app, env);
            _container.Verify();

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

        private void RegisterSimpleInjector(IServiceCollection services)
        {
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));

            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        private void InitializeContainer(IApplicationBuilder app, IHostingEnvironment env)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Add application presentation components:
            _container.RegisterMvcControllers(app);

            RegisterUseCases();
            RegisterRepositories(env);
        }

        private void RegisterUseCases()
        {
            _container.Register<IRenderReportUseCase, RenderReportUseCase>();
        }

        private void RegisterRepositories(IHostingEnvironment env)
        {
            var webRoot = GetRootPath(env);
            var templateLocation = Path.Combine(webRoot, "Reporting", "Templates");
            var nodeAppLocation = Path.Combine(webRoot, "Reporting" , "NodeApp");

            _container.Register<IReportRepository>(() => new ReportRepository(templateLocation, nodeAppLocation));
        }

        private string GetRootPath(IHostingEnvironment env)
        {
            return IsWebRootPathNull(env) ? env.ContentRootPath : env.WebRootPath;
        }

        private bool IsWebRootPathNull(IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.WebRootPath == null;
        }
    }
}
