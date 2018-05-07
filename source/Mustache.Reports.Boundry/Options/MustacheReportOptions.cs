using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Boundry
{
    public class MustacheReportOptions
    {
        public TemplateOptions Template { get; set; }
        public NodeAppOptions NodeApp { get; set; }

        public string LibreOfficeLocation { get; set; }
    }
}
