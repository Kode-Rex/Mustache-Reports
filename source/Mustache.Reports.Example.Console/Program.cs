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

        private static RenderReport CreateReportController(IConfigurationRoot configuration)
        {
            var wordTemplateGateway = new WordTemplateGateway(configuration);
            var pdfGateway = new PdfGateway(configuration);

            var renderReportUseCase = CreateRenderReportUseCase(wordTemplateGateway);
            var renderPdfUseCase = CreateRenderDocxToPdfUseCase(pdfGateway);
            var loggerFactory = CreateLoggerFactory();

            var reportController = new RenderReport(renderReportUseCase, renderPdfUseCase, loggerFactory);

            return reportController;
        }

        private static RenderDocxToPdfUseCase CreateRenderDocxToPdfUseCase(PdfGateway pdfGateway)
        {
            var renderPdfUseCase = new RenderDocxToPdfUseCase(pdfGateway);
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

        private static RenderWordUseCase CreateRenderReportUseCase(WordTemplateGateway wordTemplateGateway)
        {
            var renderReportUseCase = new RenderWordUseCase(wordTemplateGateway);
            return renderReportUseCase;
        }
    }
}