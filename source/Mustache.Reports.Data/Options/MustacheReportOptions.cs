using Mustache.Reports.Data.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Data
{
    public class MustacheReportOptions
    {
        public TemplateOptions Template { get; set; }
        public NodeAppOptions NodeApp { get; set; }

        public string LibreOfficeLocation { get; set; }
    }
}
