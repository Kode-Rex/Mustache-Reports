using Mustache.Reports.Boundary.Report.Excel;
using Mustache.Reports.Boundary.Report.Word;

namespace Mustache.Reports.Boundary.Report
{
    public interface IReportGateway
    {
        RenderedDocumentOutput CreateWordReport(RenderWordInput input);
        RenderedDocumentOutput CreateExcelReport(RenderExcelInput input);
    }
}