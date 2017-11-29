using Simple.Report.Server.Boundry.Rendering;
using Simple.Report.Server.Boundry.Rendering.Pdf;
using Simple.Report.Server.Boundry.Rendering.Report;

namespace Simple.Report.Server.Boundry
{
    public interface IReportRepository
    {
        RenderedDocummentOutput CreateReport(RenderReportInput input);
        RenderedDocummentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
