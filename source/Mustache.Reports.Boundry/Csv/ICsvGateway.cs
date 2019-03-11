namespace Mustache.Reports.Boundary.Csv
{
    public interface ICsvGateway
    {
        RenderedDocumentOutput ConvertToCsv(RenderCsvInput input);
    }
}
