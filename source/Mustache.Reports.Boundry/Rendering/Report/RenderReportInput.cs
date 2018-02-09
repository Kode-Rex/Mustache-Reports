namespace Mustache.Reports.Boundry.Rendering.Report
{
    public class RenderReportInput
    {
        public object JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }

        public RenderReportInput()
        {
            ReportName = "report.docx";
        }
    }
}
