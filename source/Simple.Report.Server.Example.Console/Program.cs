using Simple.Report.Server.Controllers.Console;
using Simple.Report.Server.Data.ReportRendering;
using Simple.Report.Server.Domain;

namespace Simple.Report.Server.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderReportUseCase = CreateRenderReportUseCase();
            
            var writeReportTo = "C:\\SimpleReportServer.RenderedReports";
            var reportDataFilePath = "bin\\Debug\\netcoreapp2.0\\ExampleData\\WithImagesSampleData.json";

            var reportController = new RenderReport(renderReportUseCase);
            reportController.Run(writeReportTo, reportDataFilePath);
        }

        private static RenderReportUseCase CreateRenderReportUseCase()
        {
            var reportingTemplates = "bin\\Debug\\netcoreapp2.0\\ReportRendering\\Templates";
            var nodeAppLocation = "ReportRendering\\NodeApp";

            var renderReportUseCase = new RenderReportUseCase(new ReportRepository(reportingTemplates, nodeAppLocation));
            return renderReportUseCase;
        }
    }
}