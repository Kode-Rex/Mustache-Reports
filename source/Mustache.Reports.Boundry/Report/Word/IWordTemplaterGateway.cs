namespace Mustache.Reports.Boundry.Report.Word
{
    public interface IWordGateway
    {
        RenderedDocummentOutput CreateReport(RenderWordInput input);
    }
}
