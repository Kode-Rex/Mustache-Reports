namespace Mustache.Reports.Boundry.Report.Excel
{
    public class RenderExcelInput
    {
        public object JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }
        public int SheetNumber { get; set; }

        public RenderExcelInput()
        {
            ReportName = "report.xslx";
            SheetNumber = 1;
        }
    }
}
