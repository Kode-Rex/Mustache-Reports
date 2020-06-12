using System.Collections.Generic;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ReportFactoryArguments
    {
        public string Extension { get; set; }
        public string TemplateName { get; set; }

        public string ReportJson { get; set; }
        public List<int> SheetNumbers { get; internal set; }
    }
}
