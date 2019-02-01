namespace Mustache.Reports.Boundary.Report.Excel
{
    public class RenderExcelInput
    {
        public object JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }
        public int SheetNumber { get; set; }

        public RenderExcelInput()
        {
            ReportName = "report.xlsx";
            SheetNumber = 1;
        }
    }
}
