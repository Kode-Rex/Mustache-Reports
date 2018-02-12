namespace Mustache.Reports.Boundry.Pdf
{
    public interface IPdfGateway
    {
        RenderedDocummentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
