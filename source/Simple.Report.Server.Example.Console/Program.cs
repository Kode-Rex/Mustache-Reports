using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Simple.Report.Server.Controllers.Console;
using Simple.Report.Server.Data;
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
            System.Console.ReadLine();
        }

        private static RenderReport CreateReportController(IConfigurationRoot configuration)
        {
            var reportRepository = new ReportRepository(configuration);

            var renderReportUseCase = CreateRenderReportUseCase(reportRepository);
            var renderPdfUseCase = CreateRenderDocxToPdfUseCase(reportRepository);
            var loggerFactory = CreateLoggerFactory();

            var reportController = new RenderReport(renderReportUseCase, renderPdfUseCase, loggerFactory);

            return reportController;
        }

        private static RenderDocxToPdfUseCase CreateRenderDocxToPdfUseCase(ReportRepository reportRepository)
        {
            var renderPdfUseCase = new RenderDocxToPdfUseCase(reportRepository);
            return renderPdfUseCase;
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

        private static RenderReportUseCase CreateRenderReportUseCase(ReportRepository reportRepository)
        {
            var renderReportUseCase = new RenderReportUseCase(reportRepository);
            return renderReportUseCase;
        }
    }
}