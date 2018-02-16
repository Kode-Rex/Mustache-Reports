using Mustache.Reports.Boundry.Report.Excel;
using Mustache.Reports.Boundry.Report.Word;

namespace Mustache.Reports.Boundry.Report
{
    public interface IReportGateway
    {
        RenderedDocummentOutput CreateWordReport(RenderWordInput input);
        RenderedDocummentOutput CreateExcelReport(RenderExcelInput input);
    }
}