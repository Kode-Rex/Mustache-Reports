using Mustache.Reports.Boundry.Rendering;
using Mustache.Reports.Boundry.Rendering.Pdf;
using Mustache.Reports.Boundry.Rendering.Report;

namespace Mustache.Reports.Boundry
{
    public interface IReportRepository
    {
        RenderedDocummentOutput CreateReport(RenderReportInput input);
        RenderedDocummentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
