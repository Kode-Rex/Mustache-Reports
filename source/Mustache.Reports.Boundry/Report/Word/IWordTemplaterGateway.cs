namespace Mustache.Reports.Boundry.Report.Word
{
    public interface IWordTemplaterGateway
    {
        RenderedDocummentOutput CreateReport(RenderWordInput input);
    }
}
