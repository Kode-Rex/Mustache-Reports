namespace Simple.Report.Server.Boundry.ReportRendering
{
    public interface IReportRepository
    {
        RenderedReportOutputMessage CreatePdfReport(RenderReportInputMessage inputMessage);
    }
}
