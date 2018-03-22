using System.IO;

namespace Mustache.Reports.Boundry.Pdf
{
    public class RenderPdfInput
    {
        public Stream DocumentStream { get; set; }
        public string FileName { get; set; }

        public RenderPdfInput()
        {
            FileName = "Report";
        }
    }
}