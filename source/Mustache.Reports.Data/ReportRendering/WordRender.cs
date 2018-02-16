using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class WordRender : NodePipeLineTask
    {
        public WordRender(string applicationPath, string templatePath, string templateData)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -r \"word\" ")
        {
        }
    }
}
