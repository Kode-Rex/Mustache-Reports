namespace Mustache.Reports.Boundary.Pdf
{
    public interface IPdfGateway
    {
        RenderedDocumentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
