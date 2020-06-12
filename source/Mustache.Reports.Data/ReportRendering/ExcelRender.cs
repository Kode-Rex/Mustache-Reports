using StoneAge.Synchronous.Process.Runner.PipeLineTask;
using System.Collections.Generic;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ExcelRender : NodePipeLineTask
    {
        public ExcelRender(string applicationPath, string templatePath, string templateData, List<int> sheetNumbers)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -r \"excel\" -n \"{string.Join(',', sheetNumbers)}\" ")
        {
        }

        public override int ProcessTimeout()
        {
            return -1;
        }
    }
}