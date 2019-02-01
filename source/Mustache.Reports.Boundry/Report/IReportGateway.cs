using Mustache.Reports.Boundry.Report.Excel;
using Mustache.Reports.Boundry.Report.Word;

namespace Mustache.Reports.Boundry.Report
{
    public interface IReportGateway
    {
        RenderedDocumentOutput CreateWordReport(RenderWordInput input);
        RenderedDocumentOutput CreateExcelReport(RenderExcelInput input);
    }
}