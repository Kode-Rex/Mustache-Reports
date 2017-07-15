using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Simple.Report.Server.Data.Task
{
    public class ReportRenderTask : NodePipeLineTask
    {
        public ReportRenderTask(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" ")
        {
        }
    }
}
