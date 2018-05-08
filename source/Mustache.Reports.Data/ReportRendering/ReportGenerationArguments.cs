using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ReportGenerationArguments
    {
        public string TemplatePath { get; set; }
        public string JsonPath { get; set; }
        public int SheetNumber { get; set; }
    }
}
