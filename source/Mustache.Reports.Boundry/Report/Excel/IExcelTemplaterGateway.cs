namespace Mustache.Reports.Boundry.Report.Excel
{
    public interface IExcelTemplaterGateway
    {
        RenderedDocummentOutput CreateReport(RenderExcelInput input);
    }
}
