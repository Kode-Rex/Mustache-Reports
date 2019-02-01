namespace Mustache.Reports.Boundary.Pdf
{
    public class RenderPdfInput
    {
        public string Base64DocxReport { get; set; }
        public string FileName { get; set; }

        public RenderPdfInput()
        {
            FileName = "Report";
        }
    }
}