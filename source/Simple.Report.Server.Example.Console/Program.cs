using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Simple.Report.Server.Controllers.Console;
using Simple.Report.Server.Data.ReportRendering;
using Simple.Report.Server.Domain;

namespace Simple.Report.Server.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = SetupConfiguration();

            var writeReportTo = configuration["Reporting:RenderLocation"];
            var reportDataFilePath = configuration["Reporting:RelativeSampleDataLocation"];
            var libreOfficeLocation = configuration["Reporting:LibreOfficeLocation"];

            var reportController = CreateReportController(configuration);
            reportController.Run(libreOfficeLocation, writeReportTo, reportDataFilePath);
        }

        private static RenderReport CreateReportController(IConfigurationRoot configuration)
        {
            var renderReportUseCase = CreateRenderReportUseCase(configuration);
            var loggerFactory = CreateLoggerFactory();

            var reportController = new RenderReport(renderReportUseCase, loggerFactory);

            return reportController;
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }

        private static LoggerFactory CreateLoggerFactory()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole((text, logLevel) => logLevel >= LogLevel.Trace, true);
            return loggerFactory;
        }

        private static RenderReportUseCase CreateRenderReportUseCase(IConfigurationRoot configuration)
        {
            var reportingTemplates = configuration["Reporting:RelativeReportTemplateLocation"];
            var nodeAppLocation = configuration["Reporting:RelativeToConsoleNodeAppLocation"];

            var renderReportUseCase = new RenderReportUseCase(new ReportRepository(reportingTemplates, nodeAppLocation));
            return renderReportUseCase;
        }
    }
}