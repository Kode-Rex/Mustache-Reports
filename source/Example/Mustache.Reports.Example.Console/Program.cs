using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Boundary.Options;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Excel;
using Mustache.Reports.Boundary.Report.Word;
using Mustache.Reports.Data;
using Mustache.Reports.Domain.Excel;
using Mustache.Reports.Domain.Pdf;
using Mustache.Reports.Domain.Word;
using Mustache.Reports.Example.Console.Presenter;

namespace Mustache.Reports.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = Setup_Configuration();
            var serviceProvider = Setup_Services(configuration);

            var reportController = serviceProvider.GetService<ReportController>();

            var writeReportTo = configuration["DemoOptions:RenderLocation"];
            var reportDataFilePath = configuration["DemoOptions:SampleDataLocation"];
            reportController.Run(writeReportTo,reportDataFilePath);

            System.Console.ReadLine();
        }

        private static ServiceProvider Setup_Services(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection()
                .AddScoped<IPdfGateway, PdfGateway>()
                .AddScoped<IReportGateway, ReportGateway>()
                .AddScoped<IRenderWordUseCase, RenderWordUseCase>()
                .AddScoped<IRenderExcelUseCase, RenderExcelUseCase>()
                .AddScoped<IRenderDocxToPdfUseCase, RenderWordToPdfUseCase>()
                .AddScoped<IRenderAsWordThenPdfUseCase, RenderAsWordThenPdfUseCase>()
                .AddScoped<IConsolePresenter,ConsolePresenter>()
                .AddScoped<ReportController, ReportController>()
                .Configure<MustacheReportOptions>(configuration.GetSection("MustacheReportOptions"))
                .AddLogging();

            var serviceProvider = services.BuildServiceProvider();
            Add_Logging(serviceProvider);

            return serviceProvider;
        }

        private static void Add_Logging(ServiceProvider serviceProvider)
        {
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
        }

        private static IConfigurationRoot Setup_Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}