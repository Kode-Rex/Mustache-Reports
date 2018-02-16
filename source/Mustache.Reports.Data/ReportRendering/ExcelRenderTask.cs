using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ExcelRenderTask : NodePipeLineTask
    {
        public ExcelRenderTask(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -rt \"excel\" ")
        {
        }
    }
}