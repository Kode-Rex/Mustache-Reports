namespace Mustache.Reports.Boundry.Report
{
    public interface IWordTemplaterGateway
    {
        RenderedDocummentOutput CreateReport(RenderWordInput input);
    }
}
