using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Boundary.Csv
{
    public class RenderCsvInput
    {
        public string Base64ExcelFile { get; set; }
        public string FileName { get; set; }

        public RenderCsvInput()
        {
            FileName = "CsvFile";
        }
    }
}
