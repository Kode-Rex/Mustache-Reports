using System.Collections.Generic;
using System.Linq;

namespace Mustache.Reports.Boundary.Report.Excel
{
    public class RenderExcelInput
    {
        public object JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }
        public List<int> SheetNumbers { get; set; }
        public int SheetNumber {
            get
            {
                return SheetNumbers.FirstOrDefault();
            } 
            set{
                SheetNumbers = new List<int> { value };
            } 
        }

        public RenderExcelInput()
        {
            ReportName = "report.xlsx";
            SheetNumbers = new List<int> { 1 };
        }
    }
}
