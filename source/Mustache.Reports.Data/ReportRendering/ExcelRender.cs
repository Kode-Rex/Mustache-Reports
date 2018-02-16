using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ExcelRender : NodePipeLineTask
    {
        public ExcelRender(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -rt \"excel\" ")
        {
        }
    }
}