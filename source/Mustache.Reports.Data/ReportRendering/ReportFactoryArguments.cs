using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ReportFactoryArguments
    {
        public string Extension { get; set; }
        public string TemplateName { get; set; }

        public string ReportJson { get; set; }
        public int SheetNumber { get; internal set; }
    }
}
