using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ReportRenderTask : NodePipeLineTask
    {
        public ReportRenderTask(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" ")
        {
        }
    }
}
