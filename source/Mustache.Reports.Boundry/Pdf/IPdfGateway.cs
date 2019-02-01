namespace Mustache.Reports.Boundry.Pdf
{
    public interface IPdfGateway
    {
        RenderedDocumentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
