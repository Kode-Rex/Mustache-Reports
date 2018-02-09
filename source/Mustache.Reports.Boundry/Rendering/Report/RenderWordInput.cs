namespace Mustache.Reports.Boundry.Rendering.Report
{
    public class RenderWordInput
    {
        public object JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }

        public RenderWordInput()
        {
            ReportName = "report.docx";
        }
    }
}
