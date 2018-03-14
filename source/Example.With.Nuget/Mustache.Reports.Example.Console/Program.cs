using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Data;
using Mustache.Reports.Domain;

namespace Mustache.Reports.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = SetupConfiguration();

            var writeReportTo = configuration["Reporting:RenderLocation"];
            var reportDataFilePath = configuration["Reporting:RelativeSampleDataLocation"];

            var reportController = CreateReportController(configuration);
            reportController.Run(writeReportTo, reportDataFilePath);
            System.Console.ReadLine();
        }

        private static ReportController CreateReportController(IConfigurationRoot configuration)
        {
            var reportGateway = new ReportGateway(configuration);
            var pdfGateway = new PdfGateway(configuration);

            var renderReportUseCase = CreateRenderReportUseCase(reportGateway);
            var renderPdfUseCase = CreateRenderDocxToPdfUseCase(pdfGateway);
            var loggerFactory = CreateLoggerFactory();

            var reportController = new ReportController(renderReportUseCase, renderPdfUseCase, loggerFactory);

            return reportController;
        }

        private static RenderWordToPdfUseCase CreateRenderDocxToPdfUseCase(PdfGateway pdfGateway)
        {
            var renderPdfUseCase = new RenderWordToPdfUseCase(pdfGateway);
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

        private static RenderWordUseCase CreateRenderReportUseCase(ReportGateway reportGateway)
        {
            var renderReportUseCase = new RenderWordUseCase(reportGateway);
            return renderReportUseCase;
        }
    }
}