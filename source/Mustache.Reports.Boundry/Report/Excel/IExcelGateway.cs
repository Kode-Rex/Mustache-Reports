namespace Mustache.Reports.Boundry.Report.Excel
{
    public interface IExcelGateway
    {
        RenderedDocummentOutput CreateReport(RenderExcelInput input);
    }
}
