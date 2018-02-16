using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class WordRenderTask : NodePipeLineTask
    {
        public WordRenderTask(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -rt \"word\" ")
        {
        }
    }
}
