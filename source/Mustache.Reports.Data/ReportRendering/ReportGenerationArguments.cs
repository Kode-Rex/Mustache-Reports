using System.Collections.Generic;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ReportGenerationArguments
    {
        public string TemplatePath { get; set; }
        public string JsonPath { get; set; }
        public List<int> SheetNumbers { get; set; }
    }
}
